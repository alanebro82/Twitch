using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        public PlayerViewModel( ITwitchQueryService aTwitchQueryService )
        {
            mTwitchQueryService = aTwitchQueryService;
        }

        public async Task Init( Stream aStream )
        {
            if( aStream == null )
            {
                return;
            }


            if( CurrentStream != null &&
                aStream.Channel.Name == CurrentStream.Channel.Name )
            {
                // same stream, don't reload
                return;
            }

            CurrentStream = aStream;
            var theM3uStreams = M3uStream.ParseM3uStreams( await mTwitchQueryService.GetChannel( CurrentStream.Channel.Name ) );
            mStreamLocationList.Clear();
            foreach( var theM3uStream in theM3uStreams )
            {
                mStreamLocationList.Add( theM3uStream );
            }
            SelectedStreamLocation = StreamLocationList.FirstOrDefault();
        }

        public void Stop()
        {
            Cleanup();
        }

        public override void Cleanup()
        {
            SelectedStreamLocation = null;
            mStreamLocationList.Clear();
            CurrentStream = null;
            base.Cleanup();
        }

        public IEnumerable<M3uStream> StreamLocationList
        {
            get
            {
                return mStreamLocationList;
            }
        }
        private readonly ObservableCollection<M3uStream> mStreamLocationList = new ObservableCollection<M3uStream>();

        public M3uStream SelectedStreamLocation
        {
            get
            {
                return mSelectedStreamLocation;
            }
            set
            {
                Set( nameof( SelectedStreamLocation ), ref mSelectedStreamLocation, value );
            }
        }
        private M3uStream mSelectedStreamLocation;

        public Stream CurrentStream
        {
            get
            {
                return mCurrentStream;
            }
            set
            {
                Set( nameof( CurrentStream ), ref mCurrentStream, value );
            }
        }
        private Stream mCurrentStream;


        private readonly ITwitchQueryService mTwitchQueryService;

    }
}
