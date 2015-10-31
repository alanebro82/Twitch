using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        public MainViewModel( ITwitchQueryService aTwitchQueryService )
        {
            mTwitchQueryService = aTwitchQueryService;

            LaunchPlaylistCommand = new RelayCommand<string>( async ( aChannel ) => await LaunchPlaylist( aChannel ), ( aChannel ) => !IsLaunchingPlaylist );
            GetGamesListCommand = new RelayCommand( async () => await GetGamesList() );
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

        //----------------------------------------------------------------------
        // PUBLIC COMMANDS
        //----------------------------------------------------------------------

        public RelayCommand<string> LaunchPlaylistCommand{ get; }
        public RelayCommand GetGamesListCommand{ get; }

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
                if( Set( nameof( IsLaunchingPlaylist ), ref mIsLaunchingPlaylist, value ) )
                {
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

    }
}