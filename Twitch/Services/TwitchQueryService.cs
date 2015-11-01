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

        public async Task<GameSearchResults> GetGames()
        {
            using( var theGamesListResponse = await new HttpClient().GetAsync( GamesListUri() ) )
            {
                using( var theGamesListStream = theGamesListResponse.Content )
                {
                    return new GameSearchResults( JsonObject.Parse( await theGamesListStream.ReadAsStringAsync() ) );
                }
            }
        }

        public async Task<StreamSearchResults> GetChannels( string aGame )
        {
            using( var theChannelsListResponse = await new HttpClient().GetAsync( StreamsListUri( aGame ) ) )
            {
                using( var theChannelsListStream = theChannelsListResponse.Content )
                {
                    return new StreamSearchResults( JsonObject.Parse( await theChannelsListStream.ReadAsStringAsync() ) );
                }
            }
        }

        public async Task<string> GetChannel( string aChannelName )
        {
            using( var theTokenResponse = await new HttpClient().GetAsync( TokenRequestUri( aChannelName ) ) )
            {
                using( var theTokenStream = theTokenResponse.Content )
                {
                    var theAccessToken = new AccessToken( JsonObject.Parse( await theTokenStream.ReadAsStringAsync() ) );
                    var theTokenJson = theAccessToken.Token.ToJsonObject();

                    using( var thePlaylistResponse = await new HttpClient().GetAsync( StreamRequestUri( theAccessToken.Token.Channel, theTokenJson, theAccessToken.Signature ) ) )
                    {
                        using( var thePlaylistStream = thePlaylistResponse.Content )
                        {
                            return await thePlaylistStream.ReadAsStringAsync();
                        }
                    }
                }
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

        private Uri GamesListUri()
        {
            return new Uri( "https://api.twitch.tv/kraken/games/top?limit=10" );
        }

        private Uri StreamsListUri( string aGameName )
        {
            return new Uri( $"https://api.twitch.tv/kraken/streams?game={aGameName}&limit=10" );
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
