﻿using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using YesPojiQuota.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using YesPojiQuota.Utils;
using Windows.UI.Notifications;
using YesPojiUtmLib.Services;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Helpers;
using Windows.Networking.NetworkOperators;

namespace YesPojiQuota
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += OnUnhandledException;
        }

        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await DispatcherHelper.RunAsync(async () =>
            {
                await new MessageDialog($"Application Unhandled Exception:\r\n" +
                    $"{e.Exception.Message}\r\n" +
                    $"{e.Exception.StackTrace}", "Error :(")
                    .ShowAsync();

                e.Handled = false;
            });
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Initialize(e);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            Initialize(args);
        }

        private void Initialize(IActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            DispatcherHelper.Initialize();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e is LaunchActivatedEventArgs)
            {
                var ee = e as LaunchActivatedEventArgs;
                if (ee.PrelaunchActivated == false)
                {
                    if (rootFrame.Content == null)
                    {
                        // When the navigation stack isn't restored navigate to the first page,
                        // configuring the new page by passing required information as a navigation
                        // parameter
                        rootFrame.Navigate(typeof(MainPage), ee.Arguments);
                    }
                    // Ensure the current window is active
                    Window.Current.Activate();
                }
            }

            Messenger.Default.Register<NotificationMessageAction<string>>(this, HandleNotification);
            //BackgroundTaskManager.RegisterTestTask();
            InitializeUi();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);


        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void HandleNotification(NotificationMessageAction<string> message)
        {
            //message.Execute("Success from <App.xaml>");
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
            DispatcherHelper.RunAsync(() => dialog.ShowMessage(message.Notification, "Message"));
        }

        private async void InitializeUi()
        {
            // Phone status bar settings
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                Color color = (Color)App.Current.Resources["SystemAccentColorDark1"];
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.ShowAsync();
                //color = ColourHelper.LightenDarkenColor(color, 1.05);
                statusBar.BackgroundColor = color;
                statusBar.BackgroundOpacity = 1;
            }
            else
            {
                Color color = ((SolidColorBrush)Current.Resources["SystemControlBackgroundAccentBrush"]).Color;
                ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = color;
                ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = color;
                ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = color;
                ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = color;
            }
        }
    }
}
