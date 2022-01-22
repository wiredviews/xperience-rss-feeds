using Microsoft.Extensions.DependencyInjection.Extensions;
using XperienceCommunity.RSSFeeds;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionPageTemplateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRSSFeedItemsRetreiver"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRSSFeeds<TRSSFeedItemsRetreiver>(this IServiceCollection services) where TRSSFeedItemsRetreiver : class, IRSSFeedItemsRetriever
        {
            services.AddScoped<IRSSFeedItemsRetriever, TRSSFeedItemsRetreiver>();

            return services.AddRSSFeeds();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRSSFeeds(this IServiceCollection services)
        {
            services.TryAddScoped<IRSSFeedItemsRetriever, DefaultRSSFeedItemsRetriever>();

            return services
                .AddControllersWithViews()
                .AddApplicationPart(typeof(RssFeedController).Assembly)
                .Services;
        }
    }
}
