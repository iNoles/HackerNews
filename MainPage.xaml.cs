using Microsoft.Extensions.Logging;

namespace HackerNews;

public partial class MainPage
{
    private readonly NewsViewModel _newsViewModel;
    
    public MainPage()
    {
        InitializeComponent();

        // Set up the logger
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Add other logging providers as necessary
        });

        var logger = loggerFactory.CreateLogger<NewsViewModel>();

        // Create an instance of NewsService, passing the loggerFactory if needed
        var newsService = new NewsService(loggerFactory);

        // Create the NewsViewModel instance
        _newsViewModel = new NewsViewModel(newsService, logger);

        // Set the ItemsSource for the NewsListView
        NewsListView.ItemsSource = _newsViewModel.TopStoryCollection;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _newsViewModel.Refresh();
    }

    private async void NewsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var listView = (ListView)sender;
        listView.SelectedItem = null;

        if (e.SelectedItem is not StoryModel storyModel) return;
        if (!string.IsNullOrEmpty(storyModel.Url))
        {
            var browserOptions = new BrowserLaunchOptions();
            await Browser.Default.OpenAsync(storyModel.Url, browserOptions);
        }
        else
        {
            await DisplayAlert("Invalid Article", "ASK HN articles have no url", "OK");
        }
    }
}