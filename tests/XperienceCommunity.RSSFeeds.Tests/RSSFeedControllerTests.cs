using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.XperienceCommunity;
using CMS.Tests;
using FluentAssertions;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Tests.DocumentEngine;
using WilderMinds.RssSyndication;

namespace XperienceCommunity.RSSFeeds.Tests;

public class RSSFeedControllerTests : UnitTests
{
    [Test]
    public async Task RSSFeedController_Will_Return_Not_Found_When_The_PageDataContext_Is_Not_Populated()
    {
        var log = Substitute.For<IEventLogService>();
        var contextRetriever = Substitute.For<IPageDataContextRetriever>();
        _ = contextRetriever.TryRetrieve(out Arg.Any<IPageDataContext<RSSFeedPage>>())
            .Returns(c =>
            {
                c[0] = null;

                return false;
            });

        var itemsRetriever = Substitute.For<IRSSFeedItemsRetriever>();
        var urlRetriever = Substitute.For<IPageUrlRetriever>();

        var sut = new RssFeedController(contextRetriever, itemsRetriever, urlRetriever, log);

        var result = await sut.Index(default);

        _ = result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task RSSFeedController_Will_Return_Not_Found_When_No_PageTypes_Have_Been_Selected()
    {
        Fake().DocumentType<RSSFeedPage>(RSSFeedPage.CLASS_NAME);

        var log = Substitute.For<IEventLogService>();

        var page = TreeNode.New<RSSFeedPage>().With(p =>
        {
            p.Fields.PageTypes = "";
        });

        var dataContext = Substitute.For<IPageDataContext<RSSFeedPage>>();
        _ = dataContext.Page.Returns(page);

        var contextRetriever = Substitute.For<IPageDataContextRetriever>();
        _ = contextRetriever.TryRetrieve(out Arg.Any<IPageDataContext<RSSFeedPage>>())
            .Returns(c =>
            {
                c[0] = dataContext;

                return true;
            });

        var itemsRetriever = Substitute.For<IRSSFeedItemsRetriever>();
        var urlRetriever = Substitute.For<IPageUrlRetriever>();

        var sut = new RssFeedController(contextRetriever, itemsRetriever, urlRetriever, log);

        var result = await sut.Index(default);

        _ = result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task RSSFeedController_Will_Return_A_Serialized_Feed()
    {
        Fake().DocumentType<RSSFeedPage>(RSSFeedPage.CLASS_NAME);

        var log = Substitute.For<IEventLogService>();

        var page = TreeNode.New<RSSFeedPage>().With(p =>
        {
            p.Fields.CacheLifetimeMinutes = 1;
            p.Fields.Title = "Feed Title";
            p.Fields.Description = "Feed Description";
            p.Fields.ItemsCount = 20;
            p.Fields.PageTypes = "One;Two";
        });

        var dataContext = Substitute.For<IPageDataContext<RSSFeedPage>>();
        _ = dataContext.Page.Returns(page);

        var contextRetriever = Substitute.For<IPageDataContextRetriever>();
        _ = contextRetriever.TryRetrieve(out Arg.Any<IPageDataContext<RSSFeedPage>>())
            .Returns(c =>
            {
                c[0] = dataContext;

                return true;
            });

        string absoluteUrl = "https://localhost:1234";

        var urlRetriever = Substitute.For<IPageUrlRetriever>();
        urlRetriever.Retrieve(Arg.Is(page))
            .Returns(new PageUrl { AbsoluteUrl = absoluteUrl });

        var items = new[] { new Item { Title = "Item 1" }, new Item { Title = "Item 2" } };
        var itemsRetriever = Substitute.For<IRSSFeedItemsRetriever>();
        itemsRetriever.RetrieveAsync(Arg.Is(page), Arg.Any<CancellationToken>())
            .Returns(items);

        var sut = new RssFeedController(contextRetriever, itemsRetriever, urlRetriever, log);

        var result = await sut.Index(default);

        if (result is ContentResult contentResult)
        {
            _ = contentResult.ContentType.Should().Be(RssFeedController.FEED_CONTENT_TYPE);
            _ = contentResult.Content.Should()
                .Contain("Feed Title")
                .And.Contain("Feed Description")
                .And.Contain("Item 1")
                .And.Contain("Item 2");
        }
        else
        {
            _ = result.Should().BeOfType<ContentResult>();
        }
    }
}
