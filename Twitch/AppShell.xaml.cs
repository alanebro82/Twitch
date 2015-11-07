using Microsoft.Practices.ServiceLocation;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Twitch
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell : Page
    {

        public static AppShell Current = null;

        /// <summary>
        /// Initializes a new instance of the AppShell, sets the static 'Current' reference,
        /// adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        /// provide the nav menu list with the data to display.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            Loaded += ( sender, args ) =>
            {
                Current = this;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;
            Window.Current.CoreWindow.KeyDown += HandleKeyDown;
            Window.Current.CoreWindow.SizeChanged += CoreWindow_SizeChanged;
        }

        public Frame AppFrame { get { return mFrame; } }

        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested( object sender, BackRequestedEventArgs e )
        {
            bool theHandle = e.Handled;
            BackRequested( ref theHandle );
            e.Handled = theHandle;
        }

        private void BackRequested( ref bool aHandled )
        {
            // Get a hold of the current frame so that we can inspect the app back stack.
            if( AppFrame == null )
            {
                return;
            }

            if( DesiredPlayerVerticalAlignment == VerticalAlignment.Stretch )
            {
                if( ApplicationView.GetForCurrentView().IsFullScreenMode )
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                }
                SetSplitPlayerHeight();
                aHandled = true;
            }
            else if( AppFrame.CanGoBack && !aHandled )
            {
                // If not, set the event to handled and go back to the previous page in the app.
                aHandled = true;
                AppFrame.GoBack();

                UpdateBackButtonVisibility();
            }
        }

        private void HandleKeyDown( CoreWindow sender, KeyEventArgs e )
        {
            if( e.VirtualKey == Windows.System.VirtualKey.Back ||
                e.VirtualKey == Windows.System.VirtualKey.GoBack )
            {
                bool theHandle = e.Handled;
                BackRequested( ref theHandle );
                e.Handled = theHandle;
            }
        }

        #endregion
        private void CoreWindow_SizeChanged( CoreWindow sender, WindowSizeChangedEventArgs args )
        {
            UpdatePlayerSize();
        }

        private static void UpdatePlayerSize()
        {
            switch( Current.DesiredPlayerVerticalAlignment )
            {
                case VerticalAlignment.Bottom:
                    if( Current.DesiredPlayerHeight != 0 )
                    {
                        Current.DesiredPlayerHeight = Window.Current.Bounds.Height * 0.3;
                    }
                    break;
                case VerticalAlignment.Stretch:
                    Current.DesiredPlayerHeight = ( Current.AppFrame.Parent as FrameworkElement ).Height;
                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Top:
                default:
                    break;
            }
        }

        public static void SetDefaultPlayerHeight()
        {
            if( Current.AppFrame != null )
            {
                Current.AppFrame.Visibility = Visibility.Visible;
            }
            Current.DesiredPlayerVerticalAlignment = VerticalAlignment.Bottom;
            Current.DesiredPlayerHeight = 0;
            UpdatePlayerSize();
            Current.mClosePlayerButton.Visibility = Visibility.Collapsed;

            UpdateBackButtonVisibility();
        }

        public static void SetSplitPlayerHeight()
        {
            if( Current.AppFrame != null )
            {
                Current.AppFrame.Visibility = Visibility.Visible;
            }

            Current.DesiredPlayerVerticalAlignment = VerticalAlignment.Bottom;
            Current.DesiredPlayerHeight = 200;
            UpdatePlayerSize();
            Current.mClosePlayerButton.Visibility = Visibility.Visible;

            UpdateBackButtonVisibility();
        }

        public static void SetFullPlayerHeight()
        {
            if( Current.AppFrame != null )
            {
                Current.AppFrame.Visibility = Visibility.Collapsed;
            }

            Current.DesiredPlayerVerticalAlignment = VerticalAlignment.Stretch;
            Current.DesiredPlayerHeight = ( Current.AppFrame.Parent as FrameworkElement ).Height;
            UpdatePlayerSize();
            Current.mClosePlayerButton.Visibility = Visibility.Collapsed;

            UpdateBackButtonVisibility();
        }

        private static void UpdateBackButtonVisibility()
        {
            var theNavManager = SystemNavigationManager.GetForCurrentView();
            if( Current.DesiredPlayerVerticalAlignment == VerticalAlignment.Stretch )
            {
                theNavManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else if( Current.AppFrame != null &&
                     Current.AppFrame.CanGoBack )
            {
                theNavManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                theNavManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        public VerticalAlignment DesiredPlayerVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue( DesiredPlayerVerticalAlignmentProperty ); }
            set { SetValue( DesiredPlayerVerticalAlignmentProperty, value ); }
        }
        public static readonly DependencyProperty DesiredPlayerVerticalAlignmentProperty =
            DependencyProperty.Register( "DesiredPlayerVerticalAlignment", typeof( VerticalAlignment ), typeof( AppShell ), new PropertyMetadata( VerticalAlignment.Bottom ) );


        public double DesiredPlayerHeight
        {
            get { return (double)GetValue( DesiredPlayerHeightProperty ); }
            set { SetValue( DesiredPlayerHeightProperty, value ); }
        }
        public static readonly DependencyProperty DesiredPlayerHeightProperty =
            DependencyProperty.Register( "DesiredPlayerHeight", typeof( double ), typeof( AppShell ), new PropertyMetadata( 0 ) );

        private void HandleClosePlayerButtonClick( object sender, RoutedEventArgs e )
        {
            ServiceLocator.Current.GetInstance<PlayerViewModel>().Stop();
            SetDefaultPlayerHeight();
        }
    }
}
