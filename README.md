# .NET MAUI Blazor/Hybrid Firebase Cloud Messaging (FCM) MVP

A minimal viable project demonstrating push notifications for iOS and Android using FCM API V1, featuring both custom backend integration and analytics tracking.

## 🌟 Key Features

- Push notification support for Android (iOS coming soon)
- Firebase Analytics integration
- Custom backend for notification management
- Support for both push and data notifications
- Compatible with physical devices and emulators

## 🚀 Getting Started

### Prerequisites

- .NET MAUI development environment
- Firebase project with FCM enabled
- Basic understanding of Firebase Cloud Messaging

### Setup

1. Clone this repository
2. Configure your Firebase credentials
3. Set your API address in the configuration
4. Build and run the project

### Usage

1. Launch the application and capture the FCM token from the console output
2. Use the Swagger endpoint `RegisterToken` to register the captured token
3. Repeat for all target devices
4. Use the "Test to All" button in the frontend sender application to broadcast notifications

## 📊 Monitoring

### FCM Dashboard
Monitor your notification metrics in the Firebase Console:

<img width="1455" alt="FCM Dashboard Metrics" src="https://github.com/user-attachments/assets/2b282af8-f1be-4b3d-92ff-b862d92a6854">

### Analytics Dashboard
Track user engagement and notification performance:

![Analytics Dashboard](https://github.com/user-attachments/assets/35d3a8de-6f9e-4888-8285-ff87d5e3f2fb)

## ⚠️ Known Issues & Solutions

1. **Path Length Limitation**
   If you encounter "can't find part of path" errors with FirebaseMessaging nuget package:
   - Follow the instructions to remove path limit: [Microsoft Docs - Maximum File Path Limitation](https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=powershell)

2. **Release Build API Connection**
   - HTTP connection to API may not work in release builds
   - Workaround: Get the token from output window (using logcat) and manually register via Swagger endpoint

3. **Topic Notification**
   - On physical devices, sending a direct notification to the device might be required before topic notifications start working

## 🔍 Development Notes

- Successfully tested on:
  - Physical Android devices
  - Android emulators (Mostly debug, not all functions work)
  - Both debug and release builds
- iOS implementation is planned for future releases

## 💡 Background

This project was created after encountering numerous outdated or non-functional implementations using Shiny or Plugin.Firebase. It combines both packages to provide a working solution for FCM and Firebase Analytics integration.

## 📞 Support

If you encounter any issues or need assistance, feel free to:
- Open an issue on GitHub
- Contact me at info@reynethan.com

## 🛠️ Technologies Used

- .NET MAUI Blazor/Hybrid
- Shiny
- Plugin.Firebase
- Firebase Cloud Messaging API V1
- Firebase Analytics

## 📝 License

.. None
