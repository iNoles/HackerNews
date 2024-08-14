using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace HackerNews;

public class NewsViewModel: ObservableObject
{
    public ObservableCollection<StoryModel> TopStoryCollection { get; } = [];
    
    private readonly NewsService _newsService = new();

    public async Task Refresh()
    {
        TopStoryCollection.Clear();

        await foreach (var story in GetTopStories().ConfigureAwait(false))
        {
            InsertIntoSortedCollection(TopStoryCollection, (a, b) => b.Score.CompareTo(a.Score), story);
            //Console.WriteLine(story.Title);
        }
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
        var topStories = await _newsService.GetTopStoryAsJson();
        var topStoryJson = JsonConvert.DeserializeObject<List<string>>(topStories);
        var getTopStoryTaskList = topStoryJson.Select(x => x.ToString()).ToList();
        foreach (var topStoryId in getTopStoryTaskList)
        {
            var story = await _newsService.GetTopStory(topStoryId);
            yield return story;
        }
    }
}