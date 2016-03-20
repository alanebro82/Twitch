using System.Collections.Generic;
using System.Threading.Tasks;
using Twitch.Model;

namespace Twitch.Services
{
    //==========================================================================
    public interface ITwitchQueryService
    {
        Task<GameSearchResults> GetGames( uint aOffset, uint aSize );
        Task<StreamSearchResults> GetChannels( string aGame, uint aOffset, uint aSize );
        Task<IEnumerable<M3uStream>> GetChannel( string aChannelName );
    }
}
