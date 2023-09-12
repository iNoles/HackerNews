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
}