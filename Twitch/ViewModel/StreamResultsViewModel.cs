using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class StreamResultsViewModel : ViewModelBase
    {

        public StreamResultsViewModel( ITwitchQueryService aTwitchQueryService )
        {
            mTwitchQueryService = aTwitchQueryService;
            InitCommand = new RelayCommand<Game>( async ( aGame ) => await Init( aGame ) );
            SelectStreamCommand = new RelayCommand<Stream>( async ( aStream ) => await SelectStream( aStream ) );
        }

        private async Task Init(Game aGame)
        {
            var theChannels = await mTwitchQueryService.GetChannels( aGame.Name );

            foreach( var theStream in theChannels.StreamsList )
            {
                mStreams.Add( theStream );
            }
        }

        private async Task SelectStream( Stream aStream )
        {
            await mTwitchQueryService.LaunchChannel( aStream.Channel.Name );
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

        public IEnumerable<Stream> Streams
        {
            get
            {
                return mStreams;
            }
        }
        private ObservableCollection<Stream> mStreams = new ObservableCollection<Stream>();

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private readonly ITwitchQueryService mTwitchQueryService;
    }
}
