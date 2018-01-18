
Download dotnet.exe tools `https://docs.microsoft.com/en-us/dotnet/core/tools/index?tabs=netcore2x` and install.

Edit `app.config` modifying the appSetting <add> elements to suit your local MQ instance

Run `..\..\WebSphereMqClient\build.cmd` to create local nuget package

Run `build.cmd`
- Requires nuget.exe in path

The test inside MessageQueueTest.cs will connect to the host/channel/port/manager/queue, and place a message onto the queue, then read it off again.