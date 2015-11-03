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
    public class StreamResultsViewModel : ViewModelBase
    {

        public StreamResultsViewModel( INavigationService aNavService, ITwitchQueryService aTwitchQueryService )
        {
            mNavService = aNavService;
            mTwitchQueryService = aTwitchQueryService;
            SelectStreamCommand = new RelayCommand<Stream>( SelectStream );
        }

        public Task Init(Game aGame)
        {
            Game = aGame;
            if( Game == null )
            {
                if( mNavService.CurrentPageKey == ViewModelLocator.scStreamResultsPageKey )
                {
                    mNavService.GoBack();
                }
            }

            mStreams = new IncrementalLoadingCollection<Stream>( GetStreams );
            RaisePropertyChanged( nameof( Streams ) );

            return Task.FromResult( 0 );
        }

        public override void Cleanup()
        {
            mStreams = null;
            base.Cleanup();
        }

        private void SelectStream( Stream aStream )
        {
            if( aStream == null )
            {
                return;
            }

            mNavService.NavigateTo( ViewModelLocator.scPlayerPageKey, aStream );
        }

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
        // PUBLIC COMMANDS
        //----------------------------------------------------------------------

        public RelayCommand<Game> InitCommand { get; }

        public RelayCommand<Stream> SelectStreamCommand { get; }

        

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
                Set( nameof( Game ), ref mGame, value );
            }
        }
        private Game mGame = null;

        public IncrementalLoadingCollection<Stream> Streams
        {
            get
            {
                return mStreams;
            }
        }
        private IncrementalLoadingCollection<Stream> mStreams;

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly INavigationService mNavService;
        private readonly ITwitchQueryService mTwitchQueryService;
    }

}
