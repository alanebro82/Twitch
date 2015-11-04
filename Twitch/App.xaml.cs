﻿using System;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Twitch
{
    sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched( LaunchActivatedEventArgs e )
        {
            var theRootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if( theRootFrame == null )
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                theRootFrame = new Frame();

                theRootFrame.NavigationFailed += OnNavigationFailed;

                if( e.PreviousExecutionState == ApplicationExecutionState.Terminated )
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = theRootFrame;
            }

            if( theRootFrame.Content == null )
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                theRootFrame.Navigate( typeof( MainPage ), e.Arguments );
            }
            // Ensure the current window is active
            Window.Current.Activate();
            DispatcherHelper.Initialize();

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;
            Window.Current.CoreWindow.KeyDown += HandleKeyDown;
        }

        private void SystemNavigationManagerBackRequested( object sender, BackRequestedEventArgs e )
        {
            var theRootFrame = Window.Current.Content as Frame;

            if( theRootFrame != null &&
                theRootFrame.CanGoBack )
            {
                e.Handled = true;
                theRootFrame.GoBack();
            }
        }

        private void HandleKeyDown( CoreWindow sender, KeyEventArgs e )
        {
            if( e.VirtualKey == Windows.System.VirtualKey.Back ||
                e.VirtualKey == Windows.System.VirtualKey.GoBack )
            {
                ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed( object sender, NavigationFailedEventArgs e )
        {
            throw new Exception( "Failed to load Page " + e.SourcePageType.FullName );
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending( object sender, SuspendingEventArgs e )
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            SystemNavigationManager.GetForCurrentView().BackRequested -= SystemNavigationManagerBackRequested;
            Window.Current.CoreWindow.KeyDown -= HandleKeyDown;

            deferral.Complete();
        }
    }
}
