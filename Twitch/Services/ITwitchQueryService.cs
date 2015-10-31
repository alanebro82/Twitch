using System.Threading.Tasks;
using Twitch.Model;

namespace Twitch.Services
{
    public interface ITwitchQueryService
    {
        Task<GameSearchResults> GetGames();
        Task GetChannels( string aGame );
        Task LaunchChannel( string aChannelName );
    }
}
