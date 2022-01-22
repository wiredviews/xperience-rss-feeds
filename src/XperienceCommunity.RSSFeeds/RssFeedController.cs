using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IPageDataContextRetriever dataContextRetriever;
        private readonly IRSSFeedItemsRetriever itemsRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public RssFeedController(IPageDataContextRetriever dataContextRetriever, IRSSFeedItemsRetriever itemsRetriever, IPageUrlRetriever pageUrlRetriever)
        {
            this.itemsRetriever = itemsRetriever ?? throw new ArgumentNullException(nameof(itemsRetriever));
            this.dataContextRetriever = dataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public async Task<ActionResult> Index(CancellationToken token = default)
        {
            if (!dataContextRetriever.TryRetrieve<RSSFeedPage>(out var data))
            {
                return NotFound();
            }

            var page = data.Page;

            if (page.Fields.PageTypes.Length == 0)
            {
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

            return Content(feed.Serialize(), "application/rss+xml");
        }
    }
}
