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
    //==========================================================================
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StreamResultsPage
    {
        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public StreamResultsViewModel Vm => (StreamResultsViewModel)DataContext;

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public StreamResultsPage()
        {
            this.InitializeComponent();
        }

        //----------------------------------------------------------------------
        // PROTECTED OVERRIDES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        protected override void OnNavigatedTo( NavigationEventArgs e )
        {
            base.OnNavigatedTo( e );

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Vm.Game = e.Parameter as Game;
        }

        //----------------------------------------------------------------------
        // PRIVATE EVENT HANDLERS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private void GridView_HandleItemClick( object sender, ItemClickEventArgs e )
        {
            AppShell.Current.AppFrame.Navigate( typeof( PlayerPage ), e.ClickedItem as Stream );
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
            var theColumns = Math.Max( 1, Math.Floor( theUsableWidth / scInitialWidthSize ) );

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
            DependencyProperty.Register( "DesiredItemWidth", typeof( double ), typeof( StreamResultsPage ), new PropertyMetadata( scInitialWidthSize ) );

        //----------------------------------------------------------------------
        public double DesiredItemHeight
        {
            get { return (double)GetValue( DesiredItemHeightProperty ); }
            set { SetValue( DesiredItemHeightProperty, value ); }
        }
        public static readonly DependencyProperty DesiredItemHeightProperty =
            DependencyProperty.Register( "DesiredItemHeight", typeof( double ), typeof( StreamResultsPage ), new PropertyMetadata( scInitialWidthSize / scWidthToHeightRatio ) );

        //----------------------------------------------------------------------
        // PRIVATE CONSTS
        //----------------------------------------------------------------------

        const double scInitialWidthSize = 500;
        const double scWidthToHeightRatio = 65.0 / 37.0;

    }
}
