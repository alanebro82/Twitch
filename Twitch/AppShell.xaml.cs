using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Twitch
{
    //==========================================================================
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell : Page
    {

        //----------------------------------------------------------------------
        // PUBLIC STATIC MEMBERS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        /// <summary>
        /// Current frame, used for navigating.
        /// </summary>
        public static AppShell Current
        {
            get;
            private set;
        }

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public AppShell()
        {
            InitializeComponent();

            Current = this;

            SystemNavigationManager.GetForCurrentView().BackRequested += HandleBackRequested;
            Window.Current.CoreWindow.KeyDown += HandleKeyDown;
        }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public Frame AppFrame => mFrame;

        //----------------------------------------------------------------------
        // PRIVATE EVENT HANDLERS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private void HandleBackRequested( object sender, BackRequestedEventArgs e )
        {
            bool handled = e.Handled;
            BackRequested( ref handled );
            e.Handled = handled;
        }

        //----------------------------------------------------------------------
        private void HandleKeyDown( CoreWindow sender, KeyEventArgs e )
        {
            if( e.VirtualKey == Windows.System.VirtualKey.Back ||
                e.VirtualKey == Windows.System.VirtualKey.GoBack )
            {
                bool theHandle = e.Handled;
                BackRequested( ref theHandle );
                e.Handled = theHandle;
            }
            else if( e.VirtualKey == Windows.System.VirtualKey.Escape )
            {
                var theCurrentView = ApplicationView.GetForCurrentView();
                if( theCurrentView.IsFullScreenMode )
                {
                    theCurrentView.ExitFullScreenMode();
                }
            }
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private void BackRequested( ref bool aHandled )
        {
            // Get a hold of the current frame so that we can inspect the app back stack.
            if( AppFrame == null )
            {
                return;
            }

            if( AppFrame.CanGoBack && !aHandled )
            {
                // If not, set the event to handled and go back to the previous page in the app.
                aHandled = true;
                AppFrame.GoBack();

                UpdateBackButtonVisibility();
            }
        }

        //----------------------------------------------------------------------
        private static void UpdateBackButtonVisibility()
        {
            var theNavManager = SystemNavigationManager.GetForCurrentView();
            if( Current.AppFrame != null &&
                Current.AppFrame.CanGoBack )
            {
                theNavManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                theNavManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
