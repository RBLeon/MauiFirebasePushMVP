# .NET MAUI Firebase cloud messaging Push Notifications MVP
 A MVP project for sending push notifications to iOS and Android using FCM Api V1, from your own backend.

 To make this work you capture the token from the console. After that you use swagger to enter the token using RegisterToken endpoint. After which you do this for all devices you want to send the notification to. Then simply use the test to all button in the frontend sender application and voila.

Side note: strange thing was that for the physical device I first had to send a notification directly to that device before sending to the topic started working

Also don't forget to set your own api address and your credentials for fcm. Haven't tested it for APN/iOS yet.

 If you have problems with the FirebaseMessaging nuget package giving you "can't find part of path" errors try to remove the path limit like instructed here: https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=powershell
