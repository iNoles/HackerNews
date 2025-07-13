using HackerNews.Core;
using System;
using Xunit;

namespace HackerNews.Tests;

public class StoryModelTests
{
    [Fact]
    public void CreatedAt_ReturnsCorrectLocalDateTime()
    {
        // Arrange
        var unixTime = 1620000000L; // Example Unix timestamp
        var story = new StoryModel(1, "author", 10, unixTime, "Title", "http://url");

        // Act
        var createdAt = story.CreatedAt;

        // Assert
        var expectedDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        Assert.Equal(expectedDateTime, createdAt);
    }
}
