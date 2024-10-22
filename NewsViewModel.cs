using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HackerNews;

public class NewsViewModel(NewsService newsService, ILogger<NewsViewModel> logger) : ObservableObject
{
    public ObservableCollection<StoryModel> TopStoryCollection { get; } = new ObservableCollection<StoryModel>();

    private readonly NewsService _newsService = newsService;
    private readonly ILogger<NewsViewModel> _logger = logger;

    public async Task Refresh()
    {
        // Ensure this method is called on the UI thread
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                TopStoryCollection.Clear();

                await foreach (var story in GetTopStories().ConfigureAwait(false))
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

    async IAsyncEnumerable<StoryModel> GetTopStories()
    {
        var stories = new List<StoryModel>();

        try
        {
            var topStories = await _newsService.GetTopStoryAsJson();
            var topStoryJson = JsonConvert.DeserializeObject<List<string>>(topStories);

            foreach (var topStoryId in topStoryJson)
            {
                var story = await _newsService.GetTopStory(topStoryId);
                stories.Add(story);  // Add to the temporary list
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching top stories.");
            throw;
        }

        // Yield the results after the try-catch block
        foreach (var story in stories)
        {
            yield return story;
        }
    }
}
