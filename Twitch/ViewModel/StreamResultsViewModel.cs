using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class StreamResultsViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        public StreamResultsViewModel( ITwitchQueryService aTwitchQueryService )
        {
            mTwitchQueryService = aTwitchQueryService;
        }

        public override void Cleanup()
        {
            Streams = null;
            base.Cleanup();
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        private async Task<IEnumerable<Stream>> GetStreams( uint aOffset, uint aSize )
        {
            var theGame = Game;
            if( theGame == null )
            {
                return Enumerable.Empty<Stream>();
            }

            return ( await mTwitchQueryService.GetChannels( theGame.Name, aOffset, aSize ) ).StreamsList;
        }

        //----------------------------------------------------------------------
        // PUBLIC PROPERTIES
        //----------------------------------------------------------------------

        public Game Game
        {
            get
            {
                return mGame;
            }
            set
            {
                if( Set( nameof( Game ), ref mGame, value ) )
                {
                    Streams = new IncrementalLoadingCollection<Stream>( GetStreams );
                }
            }
        }
        private Game mGame = null;

        public IncrementalLoadingCollection<Stream> Streams
        {
            get
            {
                return mStreams;
            }
            set
            {
                Set( nameof( Streams ), ref mStreams, value );
            }
        }
        private IncrementalLoadingCollection<Stream> mStreams;

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly ITwitchQueryService mTwitchQueryService;
    }

}
