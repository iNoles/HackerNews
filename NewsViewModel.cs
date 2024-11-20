using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HackerNews;

public class NewsViewModel(NewsService newsService, ILogger<NewsViewModel> logger) : ObservableObject
{
    public ObservableCollection<StoryModel> TopStoryCollection { get; } = [];

    private readonly NewsService _newsService = newsService;
    private readonly ILogger<NewsViewModel> _logger = logger;

    public async Task RefreshAsync()
    {
        // Ensure this method is called on the UI thread
        await MainThread.InvokeOnMainThreadAsync(async () =>
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
                _logger.LogError(ex, "Error while refreshing top stories.");
            }
        });
    }

    private async IAsyncEnumerable<StoryModel> GetTopStoriesAsync()
    {
        List<string> topStoryIds;
        try
        {
            var topStoriesJson = await _newsService.GetTopStoryAsJsonAsync();
            topStoryIds = JsonConvert.DeserializeObject<List<string>>(topStoriesJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching top stories.");
            yield break; // Stop if there’s an issue fetching the top story IDs
        }

        foreach (var id in topStoryIds)
        {
            StoryModel story;
            try
            {
                story = await _newsService.GetTopStoryAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching story with ID: {StoryId}", id);
                continue; // Skip this story if there's an error fetching it
            }

            if (story != null)
            {
                yield return story; // Yield only if successfully fetched
            }
        }
    }
}
