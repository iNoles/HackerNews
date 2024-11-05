namespace HackerNews;

public record StoryModel(long Id, string By, long Score, long Time, string Title, string Url)
{
    public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(Time).LocalDateTime;
}