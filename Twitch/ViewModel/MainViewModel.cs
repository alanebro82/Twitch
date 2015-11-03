using System.Collections.Generic;
using System.Linq;
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

        public void Init()
        {
            mGames = new IncrementalLoadingCollection<GameSource, Game>( 10, GetGames );
            RaisePropertyChanged( nameof( Games ) );
        }

        public override void Cleanup()
        {
            mGames = null;
            base.Cleanup();
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        private async Task<IEnumerable<Game>> GetGames( uint aOffset, uint aSize )
        {
            return ( await mTwitchQueryService.GetGames( aOffset, aSize ) ).GamesList;
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

        public RelayCommand<Game> SelectGameCommand { get; }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        public IncrementalLoadingCollection<GameSource, Game> Games
        {
            get
            {
                return mGames;
            }
        }
        private IncrementalLoadingCollection<GameSource, Game> mGames;

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly INavigationService mNavService;
        private readonly ITwitchQueryService mTwitchQueryService;
    }

    public class GameSource : IIncrementalSource<Game>
    {
        private List<Game> mGames = new List<Game>();

        public IEnumerable<Game> GetPagedItems( int pageIndex, int pageSize )
        {
            return mGames.Skip( pageIndex * pageSize ).Take( pageSize );
        }
    }
}
