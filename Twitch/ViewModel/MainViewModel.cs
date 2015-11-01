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

            SelectGameCommand = new RelayCommand<Game>( SelectGame );
        }
        public async Task Init()
        {
            mGames.Clear();
            var theGameResults = await mTwitchQueryService.GetGames();

            foreach( var theGame in theGameResults.GamesList )
            {
                mGames.Add( theGame );
            }
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

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

        public RelayCommand<Game> SelectGameCommand { get; }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

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

        private readonly INavigationService mNavService;
        private readonly ITwitchQueryService mTwitchQueryService;
    }
}
