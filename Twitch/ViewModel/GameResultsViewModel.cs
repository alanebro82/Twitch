using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    //==========================================================================
    public class GameResultsViewModel : ViewModelBase
    {

        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public GameResultsViewModel( ITwitchQueryService aTwitchQueryService )
        {
            mTwitchQueryService = aTwitchQueryService;
        }

        //----------------------------------------------------------------------
        public void Init()
        {
            Games = new IncrementalLoadingCollection<Game>( GetGames, TimeSpan.FromSeconds( 30 ) );
            RaisePropertyChanged( () => Games );
        }

        //----------------------------------------------------------------------
        public override void Cleanup()
        {
            Games = null;
            base.Cleanup();
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private async Task<IEnumerable<Game>> GetGames( uint aOffset, uint aSize )
        {
            return ( await mTwitchQueryService.GetGames( aOffset, aSize ) ).GamesList;
        }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public IncrementalLoadingCollection<Game> Games
        {
            get
            {
                return mGames;
            }
            private set
            {
                var theOldVal = mGames;
                if( value != theOldVal && theOldVal != null )
                {
                    theOldVal.Dispose();
                }
                mGames = value;
            }
        }
        private IncrementalLoadingCollection<Game> mGames;

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly ITwitchQueryService mTwitchQueryService;
    }

}
