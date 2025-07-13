using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace HackerNews.Core;

public class NewsViewModel(
    NewsService newsService,
    ILogger<NewsViewModel> logger,
    IMainThreadDispatcher dispatcher
) : ObservableObject
{
    public ObservableCollection<StoryModel> TopStoryCollection { get; } = [];

    public async Task RefreshAsync()
    {
        // Ensure this method is called on the UI thread
        await dispatcher.RunOnMainThreadAsync(async () =>
        {
            try
            {
                TopStoryCollection.Clear();

                var sortedStories = (await GetTopStoriesAsync().ToListAsync())
                    .OrderByDescending(story => story.Score)
                    .ToList();

                foreach (var story in sortedStories)
                {
                    TopStoryCollection.Add(story); // Add sorted stories to the collection
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while refreshing top stories.");
            }
        });
    }

    private async IAsyncEnumerable<StoryModel> GetTopStoriesAsync()
    {
        List<string> topStoryIds;
        try
        {
            var topStoriesJson = await newsService.GetTopStoryAsJsonAsync();
            topStoryIds = JsonSerializer.Deserialize<List<string>>(topStoriesJson) ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching top stories.");
            yield break; // Stop if there’s an issue fetching the top story IDs
        }

        foreach (var id in topStoryIds)
        {
            StoryModel story;
            try
            {
                story = await newsService.GetStoryAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching story with ID: {StoryId}", id);
                continue; // Skip this story if there's an error fetching it
            }

            if (story != null)
            {
                yield return story; // Yield only if successfully fetched
            }
        }
    }
}
