# SteamTools

[![DeepSource](https://app.deepsource.com/gh/naumenkoff/SteamTools.svg/?label=active+issues&show_trend=true&token=rLl5xSYT-6Da5CjmtVUU6Mfi)](https://app.deepsource.com/gh/naumenkoff/SteamTools/?ref=repository-badge)

Introducing **SteamTools** - the ultimate tool for Steam enthusiasts! Get instant access to any Steam profile data without using slow conversion sites.
Easily view your local accounts and scan relevant directories for quick results.
Find all files with your SteamID signature and more with SteamTools!

> If you want to suggest something, report a bug, or anything like that, just create an issue!

## Features

- You can obtain information about any Steam profile faster than by using any conversion sites.
- You can view your local accounts. If you have ever logged into a Steam account, it will be considered local and most likely has left many traces, which this tool will identify and show you.
- You can scan all relevant Steam directories and quickly get the corresponding results. If you want to see all the files where the signature of your account [SteamID] is found, this tool can help you with that.

## Installation

To use this tool at this stage, you need:
- Open Visual Studio / JetBrains Rider
- In Visual Studio:
    - Click on **Clone Repo**
    - Paste the link to my repository **`https://github.com/naumenkoff/SteamTools.git`** into the input field
    - Click on **Clone**
- In Rider:
    - Click on **Get from VCS**
    - Select **Repository URL**
    - Paste the link to my repository **`https://github.com/naumenkoff/SteamTools.git`** into the input field
    - Click on **Clone**
- Find the **SteamTools.UI** project
- Find the **appsettings.json** file and paste your [Steam Web API Key] there
- Compile and use it!

## Roadmap

> This roadmap is not final and will be updated as I remember and reflect on what I have already implemented and what I want to implement.

- [x] Move the project to WPF
- [x] Implement a user interface for the ProfileDataFetcher project (beautiful display of results, history of requests)
- [ ] Implement a user interface for the LocalProfileScanner project (a beautiful and informative list of found accounts, viewing all results from ILocalProfile through the UI)
- [x] Implement a user interface for the IDScanner project (settings for parallel scanning, file size, extensions, scanning results)
- [x] Style the MainWindow to match Windows 11: Mica & **Round Corners**
- [x] Implement custom controls in the WinUI style
    - [x] CheckBox
    - [x] TextBox
    - [x] ScrollViewer
    - [x] Button
    - [x] ToggleButton
    - [x] ListView
    - [x] Slider
- [x] Add display of profiles found in LocalProfileScanner to the history of ProfileDataFetcher and IDScanner.
- [ ] Implement beautiful notifications or ToastNotifications
- [ ] Implement the ability to interact with results in LocalProfileScanner.

   [SteamID]: <https://developer.valvesoftware.com/wiki/SteamID>
   [Steam Web API Key]: <https://steamcommunity.com/dev>








