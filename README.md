# ScrollOfTaiwuMods
[![Build status](https://img.shields.io/appveyor/ci/hanabi1224/scrolloftaiwumods/main.svg)](https://ci.appveyor.com/project/hanabi1224/scrolloftaiwumods)
[![MIT License](https://img.shields.io/github/license/hanabi1224/ScrollOfTaiwuMods.svg)](https://github.com/hanabi1224/ScrollOfTaiwuMods/blob/master/LICENSE)
========
Mod development repository for steam game scroll of taiwu. 

Once code is merged into main branch, it will automatically deploy mod packages to this [site](https://taiwumods.vercel.app/)

## Development environment setup
* Install [.NET Framework 4.5.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net452)  ([Direct download link](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net452-developer-pack-offline-installer))
* Install [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/zh-hans/downloads/) (Prefered IDE, lightweight ones like VS code also works)
* **And that is ALL. No other setup is needed, good to go now.**

## Build from IDE
* Double click on ScrollOfTaiwuMods.sln (Once VS2019 has been installed)
* devenv ScrollOfTaiwu.sln (Run VS2019 from command line)

## Build all mods from command line
* dotnet build -c Release

## Build single mod from command line
* dotnet build Mods/HelloWorld -c Release

## Publishing mod packages
* Once release build is performed successfully on certain mod projects, it automatically generate mod package under _publish folder (**Note that debug build does not publish mod package to _publish folder**).
* Once PR is merge into **main** branch, generated mod packages will be published to this [site](https://taiwumods.vercel.app/) automatically.
* Automatic update detection is enabled for UnityModManager during packaging so that mod users will be notified when new versions of mods are published.
