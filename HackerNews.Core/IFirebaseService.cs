namespace HackerNews.Core;

public interface IFirebaseService
{
    Task<string> GetTopStoriesJsonAsync();
    Task<StoryModel> GetStoryAsync(string id);
}