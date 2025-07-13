namespace HackerNews.Core;

public class NewsService(IFirebaseService firebase)
{
    public virtual async Task<string> GetTopStoryAsJsonAsync() => await firebase.GetTopStoriesJsonAsync();

    public virtual async Task<StoryModel> GetStoryAsync(string topStoryId) => await firebase.GetStoryAsync(topStoryId);
}
