using System;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core;
using CMS.DocumentEngine.Types.XperienceCommunity;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using WilderMinds.RssSyndication;
using XperienceCommunity.RSSFeeds;

[assembly: RegisterPageRoute(RSSFeedPage.CLASS_NAME, typeof(RssFeedController), ActionName = nameof(RssFeedController.Index))]

namespace XperienceCommunity.RSSFeeds
{
    /// <summary>
    /// Controller based endpoint for RSS Feed, which can return a custom feed for every <see cref="RSSFeedPage"/> in the Content Tree
    /// </summary>
    public class RssFeedController : Controller
    {
        public const string FEED_CONTENT_TYPE = "application/rss+xml";

        private readonly IPageDataContextRetriever dataContextRetriever;
        private readonly IRSSFeedItemsRetriever itemsRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IEventLogService log;

        public RssFeedController(
            IPageDataContextRetriever dataContextRetriever,
            IRSSFeedItemsRetriever itemsRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IEventLogService log)
        {
            this.itemsRetriever = itemsRetriever ?? throw new ArgumentNullException(nameof(itemsRetriever));
            this.dataContextRetriever = dataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.log = log;
        }

        public async Task<ActionResult> Index(CancellationToken token = default)
        {
            if (!dataContextRetriever.TryRetrieve<RSSFeedPage>(out var data))
            {
                log.LogError(
                    nameof(RssFeedController),
                    "MISSING_PAGE_CONTEXT",
                    $"No Page Context found for Page Type [{RSSFeedPage.CLASS_NAME}]");

                return NotFound();
            }

            var page = data.Page;

            if (page.Fields.PageTypesSet.Length == 0)
            {
                log.LogError(
                    nameof(RssFeedController),
                    "MISSING_PAGE_TYPES",
                    $"No Page Types selected for Page [{page.NodeGUID}]|[{page.NodeAliasPath}]");

                return NotFound();
            }

            var feed = new Feed()
            {
                Title = page.Fields.Title,
                Description = page.Fields.Description,
                Link = new Uri(pageUrlRetriever.Retrieve(page).AbsoluteUrl),
                Copyright = DateTime.Today.Year.ToString(),
            };

            feed.Items = await itemsRetriever.RetrieveAsync(page, token);

            return Content(feed.Serialize(), FEED_CONTENT_TYPE);
        }
    }
}
