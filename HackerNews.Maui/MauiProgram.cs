using HackerNews.Core;
using Microsoft.Extensions.Logging;

namespace HackerNews.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register services from HackerNews.Core
		builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
		builder.Services.AddSingleton<NewsService>();
		builder.Services.AddSingleton<IMainThreadDispatcher, MainThreadDispatcher>();
		builder.Services.AddTransient<NewsViewModel>();

		// Register ViewModel
		builder.Services.AddTransient<NewsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
