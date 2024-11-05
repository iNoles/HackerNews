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

                await foreach (var story in GetTopStoriesAsync())
                {
                    InsertIntoSortedCollection(TopStoryCollection, (a, b) => b.Score.CompareTo(a.Score), story);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing top stories.");
            }
        });
    }

    static void InsertIntoSortedCollection<T>(ObservableCollection<T> collection, Comparison<T> comparison, T modelToInsert)
    {
        if (collection.Count is 0)
        {
            collection.Add(modelToInsert);
        }
        else
        {
            var index = 0;
            foreach (var model in collection)
            {
                if (comparison(model, modelToInsert) >= 0)
                {
                    collection.Insert(index, modelToInsert);
                    return;
                }

                index++;
            }

            collection.Insert(index, modelToInsert);
        }
    }

    private async IAsyncEnumerable<StoryModel> GetTopStoriesAsync()
    {
        List<string> topStoryIds = [];
        
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
            StoryModel story = null;
            
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
