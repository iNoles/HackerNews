using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;

namespace HackerNews;

public class NewsService(ILoggerFactory loggerFactory)
{
    private const string FirebaseDatabaseUrl = "https://hacker-news.firebaseio.com/v0/";
    private readonly FirebaseClient _firebaseClient = new(FirebaseDatabaseUrl);

    private readonly ILogger<NewsService> _logger = loggerFactory.CreateLogger<NewsService>();

    public async Task<string> GetTopStoryAsJsonAsync()
    {
        try
         {
            return await _firebaseClient
                .Child("topstories.json?print=pretty")
                .OnceAsJsonAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching top stories from Firebase.");
             throw;
        }
    }

    public async Task<StoryModel> GetTopStoryAsync(string topStoryId)
    {
        try
        {
            return await _firebaseClient
                .Child("item")
                .Child(topStoryId)
                .Child(".json?print=pretty")
                .OnceSingleAsync<StoryModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching top story with ID: {TopStoryId}", topStoryId);
            throw;
        }
    }
}
