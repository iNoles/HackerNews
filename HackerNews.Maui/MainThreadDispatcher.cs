using HackerNews.Core;

namespace HackerNews.Maui;

public class MainThreadDispatcher : IMainThreadDispatcher
{
    public Task RunOnMainThreadAsync(Func<Task> action)
    {
        return MainThread.InvokeOnMainThreadAsync(action);
    }
}
