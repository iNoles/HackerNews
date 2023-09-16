namespace HackerNews;

public partial class MainPage
{
    private readonly NewsViewModel _newsViewModel = new();
    
    public MainPage()
    {
        InitializeComponent();
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