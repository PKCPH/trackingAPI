using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class PlayerHelper
    {
        private readonly DatabaseContext databaseContext;

        public PlayerHelper(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public List<Player> ReadAllPlayers(List<Player> PlayerList, List<PlayerTeam> PlayerTeamList)
        {
            foreach (var playerTeam in PlayerTeamList)
            {
                int index = PlayerList.FindIndex(p => p.Id == playerTeam.PlayerId);
                PlayerList[index].Teams.Add(playerTeam);
            }
            return PlayerList;
        }

        public async Task CreatePlayer(Player player)
        {
            player.Id = Guid.NewGuid();
            foreach (var playerTeam in player.Teams)
            {
                playerTeam.Id = Guid.NewGuid();
                playerTeam.PlayerId = player.Id;
                await databaseContext.PlayerTeams.AddAsync(playerTeam);
            }
            player.Teams.Clear();
            await databaseContext.Players.AddAsync(player);
            await databaseContext.SaveChangesAsync();
        }

        public Player AssignTeamsToPlayer(Player player, List<PlayerTeam> playerTeams)
        {
            foreach (var playerTeam in playerTeams)
            {
                if (player.Id == playerTeam.PlayerId)
                {
                    player.Teams.Add(playerTeam);
                }
            }
            return player;
        }

        public async Task PlayerUpdateRemovePlayerTeams(Player player, List<PlayerTeam> playerTeams)
        {
            foreach (var playerTeam in playerTeams)
            {
                if (playerTeam.PlayerId == player.Id)
                {
                    if (player.Teams.ToList().Exists(t => t.TeamId == playerTeam.TeamId) == false)
                    {
                        this.databaseContext.PlayerTeams.Remove(playerTeam);
                    }
                }
            }
        }
    }
}
