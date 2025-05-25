# Hacker News App

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
![MVVM](https://img.shields.io/badge/MVVM-CommunityToolkit-blue)
![Hacker News API](https://img.shields.io/badge/API-Hacker_News-orange)
[![.NET MAUI](https://github.com/iNoles/HackerNews/actions/workflows/maui.yml/badge.svg)](https://github.com/iNoles/HackerNews/actions/workflows/maui.yml)

This application fetches and displays the top stories from the Hacker News API, allowing users to stay updated with the latest tech news.

## Features

- Fetches the latest top stories from Hacker News (up to 500 stories)
- Displays story titles, authors, and scores
- Sorts stories by score in descending order using **LINQ**
- Easy to use and visually appealing interface
- Error handling for network and data issues

## Technologies Used

- [.NET MAUI](https://dotnet.microsoft.com/apps/maui) for cross-platform app development
- [Firebase](https://firebase.google.com/) for real-time data storage and retrieval
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/overview) for MVVM architecture
- [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json) for JSON serialization and deserialization
- [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging) for logging errors and important information
- [System.Linq.Async](https://github.com/dotnet/reactive) for asynchronous LINQ operations on `IAsyncEnumerable`
- **IAsyncEnumerable** for improved performance when fetching top stories from the Hacker News API

## Installation

To get started with the Hacker News App, follow these steps:

1. Clone this repository:
   ```bash
   git clone https://github.com/iNoles/HackerNews.git
   cd HackerNews
   ```
2. Open the project in your preferred IDE (e.g., Visual Studio).
3. Restore the NuGet packages:
    ```bash
    dotnet restore
    ```
4. Build the project:
    ```bash
    dotnet build
    ```
5. Run the application:
    ```bash
    dotnet run
    ```

## Usage

1. Launch the application.
2. The app will automatically fetch the top stories from Hacker News.
3. View the stories in a sorted list, with the ability to refresh the list to get the latest stories.

## Screenshots
![Hacker News App Desktop](https://github.com/iNoles/HackerNews/assets/49764/9f4ebdcb-014b-4979-a244-f81c6903f89b)

## Contributing

Contributions make the open-source community an amazing place to be, learn, inspire, and create. Any contributions you make are greatly appreciated.

- Fork the project.
- Create your feature branch (e.g., ``git checkout -b feature/AmazingFeature``).
- Commit your changes (e.g., ``git commit -m 'Add some AmazingFeature'``).
- Push to the branch (e.g., ``git push origin feature/AmazingFeature``).
