using System.Diagnostics.CodeAnalysis;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;

namespace HackerNews.Core;

[ExcludeFromCodeCoverage]
public class FirebaseService(ILogger<FirebaseService> logger) : IFirebaseService
{
    private readonly FirebaseClient _client = new("https://hacker-news.firebaseio.com/v0/");

    public async Task<string> GetTopStoriesJsonAsync()
    {
        try
        {
            return await _client
                .Child("topstories.json?print=pretty")
                .OnceAsJsonAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching top stories JSON from Firebase.");
            throw;
        }
    }

    public async Task<StoryModel> GetStoryAsync(string topStoryId)
    {
        try
        {
            return await _client
                .Child("item")
                .Child(topStoryId)
                .Child(".json?print=pretty")
                .OnceSingleAsync<StoryModel>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching story with ID {TopStoryId} from Firebase.", topStoryId);
            throw;
        }
    }
}
