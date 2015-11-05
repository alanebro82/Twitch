using System;
using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Twitch
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel Vm => (MainViewModel)DataContext;

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

        private void GridView_ItemClick( object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e )
        {
            var theGame = e.ClickedItem as Game;
            if( Vm.SelectGameCommand.CanExecute( theGame ) )
            {
                Vm.SelectGameCommand.Execute( theGame );
            }
        }

        public double DesiredItemWidth
        {
            get { return (double)GetValue( DesiredItemWidthProperty ); }
            set { SetValue( DesiredItemWidthProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for DesiredItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesiredItemWidthProperty =
            DependencyProperty.Register( "DesiredItemWidth", typeof( double ), typeof( MainPage ), new PropertyMetadata( scInitialWidthSize ) );

        public double DesiredItemHeight
        {
            get { return (double)GetValue( DesiredItemHeightProperty ); }
            set { SetValue( DesiredItemHeightProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for DesiredItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DesiredItemHeightProperty =
            DependencyProperty.Register( "DesiredItemHeight", typeof( double ), typeof( MainPage ), new PropertyMetadata( scInitialWidthSize / scWidthToHeightRatio ) );

        const double scInitialWidthSize = 300;
        const double scWidthToHeightRatio = 68.0 / 95.0;

        private void mGamesGridView_SizeChanged( object sender, Windows.UI.Xaml.SizeChangedEventArgs e )
        {
            var theGrid = sender as GridView;
            if( theGrid == null )
            {
                return;
            }

            var theUsableWidth = theGrid.ActualWidth - 12/*scrollbar*/;
            var theColumns = Math.Max( 2, Math.Floor( theUsableWidth / scInitialWidthSize ) );

            DesiredItemWidth = ( theUsableWidth / theColumns ) - 10/*padding size*/;
            DesiredItemHeight = DesiredItemWidth / scWidthToHeightRatio;
        }
    }
}
