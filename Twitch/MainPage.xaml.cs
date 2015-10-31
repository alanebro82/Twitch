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
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e )
        {
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
