namespace HackerNews.Core;

public interface IMainThreadDispatcher
{
    Task RunOnMainThreadAsync(Func<Task> action);
}