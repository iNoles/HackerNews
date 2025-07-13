using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HackerNews.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace HackerNews.Tests;

public class NewsViewModelTests
{
    // Fake dispatcher that runs actions on the same thread for test purposes
    private class FakeDispatcher : IMainThreadDispatcher
    {
        public Task RunOnMainThreadAsync(Func<Task> action) => action();
    }

    [Fact]
    public async Task RefreshAsync_PopulatesTopStoryCollection_SortedByScore()
    {
        // Arrange
        var mockFirebase = new Mock<IFirebaseService>();
        var stories = new List<StoryModel>
        {
            new(1, "user1", 30, 1620000000, "Lower Score", "http://example.com/1"),
            new(2, "user2", 100, 1620000001, "Higher Score", "http://example.com/2")
        };

        // Mock GetTopStoriesJsonAsync to return valid JSON
        var storyIdsJson = JsonSerializer.Serialize(new List<string> { "1", "2" });
        mockFirebase.Setup(f => f.GetTopStoriesJsonAsync())
                    .ReturnsAsync(storyIdsJson);

        // Mock GetStoryAsync to return corresponding story models
        mockFirebase.Setup(f => f.GetStoryAsync("1")).ReturnsAsync(stories[0]);
        mockFirebase.Setup(f => f.GetStoryAsync("2")).ReturnsAsync(stories[1]);

        var newsService = new NewsService(mockFirebase.Object);
        var viewModel = new NewsViewModel(newsService, NullLogger<NewsViewModel>.Instance, new FakeDispatcher());

        // Act
        await viewModel.RefreshAsync();

        // Assert
        Assert.Equal(2, viewModel.TopStoryCollection.Count);
        Assert.Equal("Higher Score", viewModel.TopStoryCollection[0].Title);
        Assert.Equal("Lower Score", viewModel.TopStoryCollection[1].Title);
    }

    [Fact]
    public async Task GetStoryAsync_ReturnsStory_WhenSuccessful()
    {
        // Arrange
        var mockFirebase = new Mock<IFirebaseService>();
        var expected = new StoryModel(1, "author", 99, 1620000000, "Test Story", "http://test");

        mockFirebase.Setup(f => f.GetStoryAsync("1")).ReturnsAsync(expected);

        var service = new NewsService(mockFirebase.Object);

        // Act
        var result = await service.GetStoryAsync("1");

        // Assert
        Assert.Equal(expected, result);
    }
}
