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
    public class StreamResultsViewModel : ViewModelBase
    {

        public StreamResultsViewModel( INavigationService aNavService, ITwitchQueryService aTwitchQueryService )
        {
            mNavService = aNavService;
            mTwitchQueryService = aTwitchQueryService;
            SelectStreamCommand = new RelayCommand<Stream>( SelectStream );
        }

        public async Task Init(Game aGame)
        {
            Game = aGame;
            if( Game == null )
            {
                mNavService.GoBack();
                return;
            }

            mStreams.Clear();

            var theChannels = await mTwitchQueryService.GetChannels( Game.Name );

            foreach( var theStream in theChannels.StreamsList )
            {
                mStreams.Add( theStream );
            }
        }

        private void SelectStream( Stream aStream )
        {
            if( aStream == null )
            {
                return;
            }

            mNavService.NavigateTo( ViewModelLocator.scPlayerPageKey, aStream );
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

        private readonly INavigationService mNavService;
        private readonly ITwitchQueryService mTwitchQueryService;
    }
}
