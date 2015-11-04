using System;
using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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



        public double DesiredItemWidth
        {
            get { return (double)GetValue( DesiredItemWidthProperty ); }
            set { SetValue( DesiredItemWidthProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for DesiredItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesiredItemWidthProperty =
            DependencyProperty.Register( "DesiredItemWidth", typeof( double ), typeof( StreamResultsPage ), new PropertyMetadata( scInitialWidthSize ) );

        public double DesiredItemHeight
        {
            get { return (double)GetValue( DesiredItemHeightProperty ); }
            set { SetValue( DesiredItemHeightProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for DesiredItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesiredItemHeightProperty =
            DependencyProperty.Register( "DesiredItemHeight", typeof( double ), typeof( StreamResultsPage ), new PropertyMetadata( scInitialWidthSize / scWidthToHeightRatio ) );

        const double scInitialWidthSize = 500;
        const double scWidthToHeightRatio = 65.0 / 37.0;

        private void mStreamsGridView_SizeChanged( object sender, Windows.UI.Xaml.SizeChangedEventArgs e )
        {
            var theGrid = sender as GridView;
            if( theGrid == null )
            {
                return;
            }

            var theUsableWidth = theGrid.ActualWidth - 12/*scrollbar*/;
            var theColumns = Math.Max( 1, Math.Floor( theUsableWidth / scInitialWidthSize ) );

            DesiredItemWidth = ( theUsableWidth / theColumns ) - ( ( theColumns - 1 ) * 4 ) / theColumns - 10;
            DesiredItemHeight = DesiredItemWidth / scWidthToHeightRatio;
        }
    }
}
