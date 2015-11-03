using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Twitch.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StreamResultsPage
    {
        public StreamResultsViewModel Vm => (StreamResultsViewModel)DataContext;

        public StreamResultsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            await Vm.Init( e.Parameter as Game );
        }

        protected override void OnNavigatingFrom( NavigatingCancelEventArgs e )
        {
            base.OnNavigatingFrom( e );
        }

        private void GridView_ItemClick( object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e )
        {
            var theStream = e.ClickedItem as Stream;
            if( Vm.SelectStreamCommand.CanExecute( theStream ) )
            {
                Vm.SelectStreamCommand.Execute( theStream );
            }
        }
    }
}
