using System;
using Twitch.Model;
using Twitch.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Twitch.View
{
    //==========================================================================
    public sealed partial class GameResultsPage : Page
    {
        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public GameResultsViewModel Vm => (GameResultsViewModel)DataContext;

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public GameResultsPage()
        {
            InitializeComponent();
        }

        //----------------------------------------------------------------------
        // PROTECTED OVERRIDES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Vm.Init();
            base.OnNavigatedTo( e );
        }

        //----------------------------------------------------------------------
        // PRIVATE EVENT HANDLERS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private void GridView_HandleItemClick( object sender, ItemClickEventArgs e )
        {
            AppShell.Current.AppFrame.Navigate( typeof( StreamResultsPage ), e.ClickedItem as Game );
        }

        //----------------------------------------------------------------------
        private void GridView_HandleSizeChanged( object sender, Windows.UI.Xaml.SizeChangedEventArgs e )
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

        //----------------------------------------------------------------------
        // PUBLIC DEPENDENCY PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public double DesiredItemWidth
        {
            get { return (double)GetValue( DesiredItemWidthProperty ); }
            set { SetValue( DesiredItemWidthProperty, value ); }
        }
        public static readonly DependencyProperty DesiredItemWidthProperty =
            DependencyProperty.Register( nameof( DesiredItemWidth ), typeof( double ), typeof( GameResultsPage ), new PropertyMetadata( scInitialWidthSize ) );

        //----------------------------------------------------------------------
        public double DesiredItemHeight
        {
            get { return (double)GetValue( DesiredItemHeightProperty ); }
            set { SetValue( DesiredItemHeightProperty, value ); }
        }
        public static readonly DependencyProperty DesiredItemHeightProperty =
            DependencyProperty.Register( nameof( DesiredItemHeight ), typeof( double ), typeof( GameResultsPage ), new PropertyMetadata( scInitialWidthSize / scWidthToHeightRatio ) );

        //----------------------------------------------------------------------
        // PRIVATE CONSTS
        //----------------------------------------------------------------------

        const double scInitialWidthSize = 300;
        const double scWidthToHeightRatio = 68.0 / 95.0;

    }
}
