# .NET MAUI Firebase cloud messaging Push Notifications MVP
 A MVP project for sending push notifications to iOS and Android using FCM Api V1, from your own backend. It needs a lot of improvement and removing redundant stuff, but it's a great starting point for getting your application to work with push and data notifications. Currenlty only tested and enabled for Android, but will be working on cleaning up the code and working on testing and enabling the iOS implementation.

 To make this work you capture the token from the console. After that you use swagger to enter the token using RegisterToken endpoint. After which you do this for all devices you want to send the notification to. Then simply use the test to all button in the frontend sender application and voila.

As you can see, you are able to see the important metrics in the FCM dashboard:
<img width="1455" alt="image" src="https://github.com/user-attachments/assets/2b282af8-f1be-4b3d-92ff-b862d92a6854">

And in the Analytics dashboard:
![Pasted Graphic](https://github.com/user-attachments/assets/35d3a8de-6f9e-4888-8285-ff87d5e3f2fb)


Side notes: 
- strange thing was that for the physical device I first had to send a notification directly to that device before sending to the topic started working
- The http connection to the api doesn't seem to work in release build (how you should be running it). For this simply get the token from the output window (I use logcat) and paste that in the swagger register endpoint.

Also don't forget to set your own api address and your credentials for fcm. Haven't tested it for APN/iOS yet.

 If you have problems with the FirebaseMessaging nuget package giving you "can't find part of path" errors try to remove the path limit like instructed here: https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=powershell
