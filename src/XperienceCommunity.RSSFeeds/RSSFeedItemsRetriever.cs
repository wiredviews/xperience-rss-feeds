using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Routing;
using CMS.DocumentEngine.Types.XperienceCommunity;
using Kentico.Content.Web.Mvc;
using WilderMinds.RssSyndication;

namespace XperienceCommunity.RSSFeeds
{
    /// <summary>
    /// Contract for retrieving RSS Feed items
    /// </summary>
    public interface IRSSFeedItemsRetriever
    {
        /// <summary>
        /// Returns RSS Feed <see cref="Item"/> instances matching the <see cref="RSSFeedPage.RSSFeedPagePageTypes"/>, limited to a maximum of <see cref="RSSFeedPage.RSSFeedPageItemsCount"/> results
        /// </summary>
        /// <param name="feedPage"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<Item>> RetrieveAsync(RSSFeedPage feedPage, CancellationToken token);
    }

    /// <summary>
    /// Default implementation of <see cref="IRSSFeedItemsRetriever"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Uses <see cref="TreeNode.DocumentPageTitle"/>, <see cref="TreeNode.DocumentPageDescription"/>, <see cref="TreeNode.DocumentPageKeyWords"/> to populate
    /// <see cref="Item"/> instances.
    /// </para>
    /// <para>
    /// Orders pages by <see cref="TreeNode.DocumentLastPublished"/>.
    /// </para>
    /// </remarks>
    public class DefaultRSSFeedItemsRetriever : IRSSFeedItemsRetriever
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public DefaultRSSFeedItemsRetriever(IPageRetriever pageRetriever, IPageUrlRetriever pageUrlRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public async Task<ICollection<Item>> RetrieveAsync(RSSFeedPage feedPage, CancellationToken token)
        {
            string[] pageTypes = feedPage.Fields.PageTypesSet;

            int itemsCount = Math.Clamp(feedPage.Fields.ItemsCount, 1, int.MaxValue);
            int cacheLifetimeMinutes = Math.Clamp(feedPage.Fields.CacheLifetimeMinutes, 0, int.MaxValue);

            var pages = await GetPagesInternal(pageTypes, itemsCount, cacheLifetimeMinutes, token);

            var items = new List<Item>(pages.Count());

            foreach (var page in pages)
            {
                string url = pageUrlRetriever.Retrieve(page)?.AbsoluteUrl ?? "";

                items.Add(new Item
                {
                    Title = page.DocumentPageTitle,
                    Body = page.DocumentPageDescription,
                    Link = new Uri(url),
                    Permalink = url,
                    PublishDate = page.DocumentLastPublished,
                    Categories = page.DocumentPageKeyWords.Split(",").Select(kw => kw.Trim()).ToArray()
                });
            }

            return items;
        }

        private Task<IEnumerable<TreeNode>> GetPagesInternal(string[] pageTypes, int count, int cacheLifetimeMinutes, CancellationToken token)
        {
            if (cacheLifetimeMinutes == 0)
            {
                var query = new MultiDocumentQuery();

                queryBuilder(query);

                return query.GetEnumerableTypedResultAsync(cancellationToken: token);
            }

            return pageRetriever.RetrieveMultipleAsync(
                queryBuilder,
                cache => cache.Key($"{nameof(DefaultRSSFeedItemsRetriever)}|{count}|{string.Join('|', pageTypes)}")
                    .Expiration(TimeSpan.FromMinutes(cacheLifetimeMinutes), true)
                    .Dependencies((results, d) =>
                    {
                        foreach (string? type in pageTypes)
                        {
                            _ = d.PageType(type);
                        }
                    }),
                cancellationToken: token);

            void queryBuilder(MultiDocumentQuery query) =>
                query
                    .WhereIn(nameof(TreeNode.ClassName), pageTypes)
                    .Columns(
                        nameof(TreeNode.DocumentPageTitle),
                        nameof(TreeNode.DocumentPageDescription),
                        nameof(TreeNode.DocumentLastPublished),
                        nameof(TreeNode.DocumentPageKeyWords)
                    )
                    .WithPageUrlPaths()
                    .OrderBy(nameof(TreeNode.DocumentLastPublished))
                    .TopN(count);
        }
    }
}
