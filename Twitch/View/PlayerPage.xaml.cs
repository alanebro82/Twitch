// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerPage
    {
        public PlayerViewModel Vm => (PlayerViewModel)DataContext;

        public PlayerPage()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Window_SizeChanged;
        }

        protected override async void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Window.Current.CoreWindow.KeyDown += HandleKeyDown;
            await Vm.Init( e.Parameter as Stream );
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e )
        {
            mMediaElement.Stop();
            Window.Current.CoreWindow.KeyDown -= HandleKeyDown;

            base.OnNavigatingFrom( e );
        }

        private void HandleKeyDown( CoreWindow sender, KeyEventArgs args )
        {
            if( args.VirtualKey == Windows.System.VirtualKey.Escape )
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
                args.Handled = true;
            }
        }

        private void Window_SizeChanged( object sender, WindowSizeChangedEventArgs e )
        {
            if( ApplicationView.GetForCurrentView().IsFullScreenMode && mMediaElement.IsFullScreen )
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
            else if( !ApplicationView.GetForCurrentView().IsFullScreenMode && !mMediaElement.IsFullScreen )
            { // do nothing
            }
        }

        private void mMediaElement_IsFullScreenChanged( object aSender, Windows.UI.Xaml.RoutedPropertyChangedEventArgs<bool> e )
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

        private void mMediaElement_DoubleTapped( object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e )
        {
            mMediaElement.IsFullScreen = !mMediaElement.IsFullScreen;
        }

    }
}
