namespace HackerNews;

public partial class MainPage : ContentPage
{
    private readonly NewsViewModel _newsViewModel;

    // Constructor accepting NewsViewModel via Dependency Injection
    public MainPage(NewsViewModel newsViewModel)
    {
        InitializeComponent();
        _newsViewModel = newsViewModel;

        // Set the BindingContext for data binding in XAML
        BindingContext = _newsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _newsViewModel.RefreshAsync();
    }

    private async void NewsCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Check if there is a selected item and access it directly by index
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is StoryModel storyModel)
        {
            // Clear selection
            ((CollectionView)sender).SelectedItem = null;
            
            if (!string.IsNullOrEmpty(storyModel.Url))
            {
                var browserOptions = new BrowserLaunchOptions { LaunchMode = BrowserLaunchMode.SystemPreferred };
                await Browser.Default.OpenAsync(storyModel.Url, browserOptions);
            }
            else
            {
                await DisplayAlert("Invalid Article", "ASK HN articles have no URL", "OK");
            }
        }
    }
}
