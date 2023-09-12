namespace HackerNews;

public record StoryModel(long Id, string By, long Score, long Time, string Title, string Url)
{
    public DateTime CreatedAt => UnixTimeStampToDateTimeOffset(Time).LocalDateTime;
    
    static DateTimeOffset UnixTimeStampToDateTimeOffset(long unixTimeStamp)
    {
        var dateTimeOffset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, default);
        return dateTimeOffset.AddSeconds(unixTimeStamp);
    }
}