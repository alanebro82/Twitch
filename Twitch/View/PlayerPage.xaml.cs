using System;
using Microsoft.PlayerFramework;
using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Twitch.View
{
    //==========================================================================
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerPage
    {
        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public PlayerViewModel Vm => (PlayerViewModel)DataContext;

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public PlayerPage()
        {
            this.InitializeComponent();

            mTimer.Interval = TimeSpan.FromSeconds( 2 );
            mTimer.Tick += HandleTimerTick;
        }

        //----------------------------------------------------------------------
        protected override async void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );

            Window.Current.SizeChanged += HandleWindowResized;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            PointerMoved += HandlePointerMoved;

            mTimer.Start();

            await Vm.Play( e.Parameter as Stream );
        }

        //----------------------------------------------------------------------
        protected override void OnNavigatedFrom( NavigationEventArgs e )
        {
            base.OnNavigatedFrom( e );

            Window.Current.SizeChanged -= HandleWindowResized;
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            PointerMoved -= HandlePointerMoved;

            mTimer.Stop();
        }

        //----------------------------------------------------------------------
        // PRIVATE EVENT HANDLERS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private void HandleWindowResized( object sender, WindowSizeChangedEventArgs e )
        {
            if( ApplicationView.GetForCurrentView().IsFullScreenMode && mMediaElement.IsFullScreen ||
                !ApplicationView.GetForCurrentView().IsFullScreenMode && !mMediaElement.IsFullScreen )
            {
                // do nothing
            }
            else if( ApplicationView.GetForCurrentView().IsFullScreenMode && !mMediaElement.IsFullScreen )
            {
                mMediaElement.IsFullScreen = true;
            }
            else if( !ApplicationView.GetForCurrentView().IsFullScreenMode && mMediaElement.IsFullScreen )
            {
                mMediaElement.IsFullScreen = false;
            }
        }

        //----------------------------------------------------------------------
        private void mMediaElement_HandleFullScreenChanged( object aSender, Windows.UI.Xaml.RoutedPropertyChangedEventArgs<bool> e )
        {
            var thePlayer = aSender as MediaPlayer;
            if( thePlayer == null )
            {
                return;
            }

            if( thePlayer.IsFullScreen )
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            }
            else
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
        }

        //----------------------------------------------------------------------
        private void mMediaElement_HandleDoubleTapped( object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e )
        {
            mMediaElement.IsFullScreen = !mMediaElement.IsFullScreen;
        }

        //----------------------------------------------------------------------
        private void HandleTimerTick( object sender, object e )
        {
            CoreWindow.GetForCurrentThread().PointerCursor = null;
        }

        //----------------------------------------------------------------------
        private void HandlePointerMoved( object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e )
        {
            if( CoreWindow.GetForCurrentThread().PointerCursor == null )
            {
                CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor( CoreCursorType.Arrow, 1 );
            }
            mTimer.Start();
        }

        //----------------------------------------------------------------------
        // PRIVATE DATA
        //----------------------------------------------------------------------

        private readonly DispatcherTimer mTimer = new DispatcherTimer();
    }
}
