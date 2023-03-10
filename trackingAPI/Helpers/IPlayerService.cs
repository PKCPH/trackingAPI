using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public interface IPlayerService
    {
        public List<Player> ReadAllPlayers(List<Player> PlayerList, List<PlayerTeam> PlayerTeamList);
        public Task CreatePlayer(Player player);
        public Player AssignTeamsToPlayer(Player player, List<PlayerTeam> playerTeams);
        public Task PlayerUpdateRemovePlayerTeams(Player player, List<PlayerTeam> playerTeams);
        public Task PlayerUpdateAddPlayerTeams(Player player, List<PlayerTeam> playerTeams);
    }
}
