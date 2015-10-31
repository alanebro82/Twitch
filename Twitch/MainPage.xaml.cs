using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Twitch
{
    public sealed partial class MainPage
    {
        public MainViewModel Vm => ( MainViewModel )DataContext;

        public MainPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;
        }

        private void MainPage_KeyDown( object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e )
        {
            if( e.Key == Windows.System.VirtualKey.Enter )
            {
                e.Handled = true;
                if( Vm.LaunchPlaylistCommand.CanExecute( mChannelName.Text ) )
                {
                    Vm.LaunchPlaylistCommand.Execute( mChannelName.Text );
                }
            }
        }

        private void SystemNavigationManagerBackRequested( object sender, BackRequestedEventArgs e )
        {
            if( Frame.CanGoBack )
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );

            this.KeyDown += MainPage_KeyDown;
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e )
        {
            this.KeyDown -= MainPage_KeyDown;

            base.OnNavigatingFrom( e );
        }

        private void GridView_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var theGame = e.ClickedItem as Game;
            if (Vm.SelectGameCommand.CanExecute(theGame))
            {
                Vm.SelectGameCommand.Execute(theGame);
            }
        }
    }
}
