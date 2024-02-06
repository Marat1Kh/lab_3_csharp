using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Notifications;
namespace lab_3.Views;

public partial class MainWindow : Window
{
    private WindowNotificationManager _notificationManager;

    public MainWindow()
    {
        InitializeComponent();
        _notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 5
        };
    }
    
    // Method to show notification
    public void ShowNotification(string title, string message, NotificationType type)
    {
        _notificationManager.Show(new Notification(title, message, type));
    }
    // Assuming this is within the MainWindow class
    public void SomeMethodThatShowsNotification()
    {
        ShowNotification("Version Info", "Your version: 1.0.0", NotificationType.Information);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}