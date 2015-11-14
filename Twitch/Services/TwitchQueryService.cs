using System;
using System.Threading.Tasks;
using Twitch.Model;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Twitch.Services
{
    //==========================================================================
    class TwitchQueryService : ITwitchQueryService
    {
        //----------------------------------------------------------------------
        // PUBLIC ITwitchQueryService
        //----------------------------------------------------------------------

        public async Task<GameSearchResults> GetGames( uint aOffset, uint aSize )
        {
            using( var theHttpClient = new HttpClient() )
            {
                return new GameSearchResults( JsonObject.Parse( await theHttpClient.GetStringAsync( GamesListUri( aOffset, aSize ) ) ) );
            }
        }

        public async Task<StreamSearchResults> GetChannels( string aGame, uint aOffset, uint aSize )
        {
            using( var theHttpClient = new HttpClient() )
            {
                return new StreamSearchResults( JsonObject.Parse( await theHttpClient.GetStringAsync( StreamsListUri( aGame, aOffset, aSize ) ) ) );
            }
        }

        public async Task<string> GetChannel( string aChannelName )
        {
            using( var theHttpClient = new HttpClient() )
            {
                var theAccessTokenJson = JsonObject.Parse( await theHttpClient.GetStringAsync( TokenRequestUri( aChannelName ) ) );
                var theTokenJson = JsonObject.Parse( theAccessTokenJson.GetNamedString( "token" ) );

                return await theHttpClient.GetStringAsync( StreamRequestUri( aChannelName, theTokenJson, theAccessTokenJson.GetNamedString( "sig" ) ) );
            }
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        private async Task<StorageFile> SaveStringToFile( string aFilename, IBuffer aContent )
        {
            var theFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync( aFilename, CreationCollisionOption.ReplaceExisting );

            using( var theStream = await theFile.OpenAsync( FileAccessMode.ReadWrite ) )
            {
                await theStream.WriteAsync( aContent );
            }

            return theFile;
        }

        private Uri GamesListUri( uint aOffset, uint aSize )
        {
            return new Uri( $"https://api.twitch.tv/kraken/games/top?offset={aOffset}&limit={aSize}" );
        }

        private Uri StreamsListUri( string aGameName, uint aOffset, uint aSize )
        {
            return new Uri( $"https://api.twitch.tv/kraken/streams?game={aGameName}&offset={aOffset}&limit={aSize}" );
        }

        private Uri TokenRequestUri( string aChannelName )
        {
            return new Uri( $"http://api.twitch.tv/api/channels/{aChannelName}/access_token" );
        }

        private Uri StreamRequestUri( string aChannelName, JsonObject aToken, string aSignature )
        {
            return new Uri( $"http://usher.twitch.tv/api/channel/hls/{aChannelName}.m3u8?player=twitchweb&&token={aToken.Stringify()}&sig={aSignature}&allow_audio_only=true&allow_source=true&type=any&p=8941" );
        }

    }
}
