using Firebase.Database;
using Firebase.Database.Query;

namespace HackerNews;

public class NewsService
{
    private const string FirebaseDatabaseUrl = "https://hacker-news.firebaseio.com/v0/";
    private readonly FirebaseClient _firebaseClient = new(FirebaseDatabaseUrl);

    public async Task<string> GetTopStoryAsJson()
    {
        return await _firebaseClient
            .Child("topstories.json?print=pretty")
            .OnceAsJsonAsync();
    }

    public async Task<StoryModel> GetTopStory(string topStoryId)
    {
        return await _firebaseClient
            .Child("item")
            .Child(topStoryId)
            .Child(".json?print=pretty")
            .OnceSingleAsync<StoryModel>();
    }
}