using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    //==========================================================================
    public class MainViewModel : ViewModelBase
    {

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        public MainViewModel( INavigationService aNavService, ITwitchQueryService aTwitchQueryService )
        {
            mNavService = aNavService;
            mTwitchQueryService = aTwitchQueryService;

            LaunchPlaylistCommand = new RelayCommand<string>( async ( aChannel ) => await LaunchPlaylist( aChannel ), ( aChannel ) => !IsLaunchingPlaylist );
            GetGamesListCommand = new RelayCommand( async () => await GetGamesList() );
            SelectGameCommand = new RelayCommand<Game>( SelectGame );
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        private async Task LaunchPlaylist( string aChannel )
        {
            try
            {
                IsLaunchingPlaylist = true;

                await mTwitchQueryService.LaunchChannel( aChannel );
            }
            finally
            {
                IsLaunchingPlaylist = false;
            }
        }

        private async Task GetGamesList()
        {
            var theGameResults = await mTwitchQueryService.GetGames();

            foreach( var theGame in theGameResults.GamesList )
            {
                mGames.Add( theGame );
            }
        }

        private void SelectGame( Game aGame )
        {
            if( aGame == null )
            {
                return;
            }

            mNavService.NavigateTo( ViewModelLocator.scStreamResultsPageKey, aGame );
        }

        //----------------------------------------------------------------------
        // PUBLIC COMMANDS
        //----------------------------------------------------------------------

        public RelayCommand<string> LaunchPlaylistCommand { get; }
        public RelayCommand GetGamesListCommand { get; }
        public RelayCommand<Game> SelectGameCommand { get; }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        private bool IsLaunchingPlaylist
        {
            get
            {
                return mIsLaunchingPlaylist;
            }
            set
            {
                if( mIsLaunchingPlaylist != value )
                {
                    mIsLaunchingPlaylist = value;
                    LaunchPlaylistCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private bool mIsLaunchingPlaylist = false;

        public IEnumerable<Game> Games
        {
            get
            {
                return mGames;
            }
        }
        private ObservableCollection<Game> mGames = new ObservableCollection<Game>();

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly ITwitchQueryService mTwitchQueryService;
        private readonly INavigationService mNavService;
    }
}
