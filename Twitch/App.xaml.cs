using System;
using GalaSoft.MvvmLight.Threading;
using Twitch.View;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Twitch
{
    //==========================================================================
    sealed partial class App
    {
        //----------------------------------------------------------------------
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched( LaunchActivatedEventArgs e )
        {
            AppShell theShell = Window.Current.Content as AppShell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if( theShell == null )
            {
                // Create a AppShell to act as the navigation context and navigate to the first page
                theShell = new AppShell();
                theShell.AppFrame.NavigationFailed += OnNavigationFailed;

                if( e.PreviousExecutionState == ApplicationExecutionState.Terminated )
                {
                    //TODO: Load state from previously suspended application
                }
            }

            // Place our app shell in the current Window
            Window.Current.Content = theShell;

            if( theShell.AppFrame.Content == null )
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                theShell.AppFrame.Navigate( typeof( GameResultsPage ), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo() );
            }

            // Ensure the current window is active
            Window.Current.Activate();
            DispatcherHelper.Initialize();
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed( object sender, NavigationFailedEventArgs e )
        {
            throw new Exception( "Failed to load Page " + e.SourcePageType.FullName );
        }

        //----------------------------------------------------------------------
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

            deferral.Complete();
        }
    }
}
