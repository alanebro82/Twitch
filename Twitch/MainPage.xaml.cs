using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Twitch
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel Vm => ( MainViewModel )DataContext;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Vm.Init();
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
