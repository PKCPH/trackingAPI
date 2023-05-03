using Microsoft.EntityFrameworkCore;
using trackingAPI.Data;
using trackingAPI.Models;

namespace trackingAPI.Helpers
{
    public class PlayerService : IPlayerService
    {
        private readonly DatabaseContext databaseContext;

        public PlayerService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public List<Player> ReadAllPlayers(List<Player> PlayerList, List<PlayerTeam> PlayerTeamList)
        {
            foreach (var playerTeam in PlayerTeamList)
            {
                int index = PlayerList.FindIndex(p => p.Id == playerTeam.PlayerId);
                if (index >= 0)
                {
                    PlayerList[index].Teams.Add(playerTeam);
                }
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
            await databaseContext.SaveChangesAsync();
        }

        public async Task PlayerUpdateAddPlayerTeams(Player player, List<PlayerTeam> playerTeams)
        {
            foreach (var team in player.Teams)
            {
                if (playerTeams.Exists(p => p.PlayerId == team.PlayerId && p.TeamId == team.TeamId) == false)
                {
                    team.Id = Guid.NewGuid();
                    await this.databaseContext.PlayerTeams.AddAsync(team);
                }
            }
            await databaseContext.SaveChangesAsync();
        }

        public async Task ChangePlayersOnTeam(List<List<PlayerTeam>> playerTeamsList)
        {
            var playerTeams = await databaseContext.PlayerTeams.ToListAsync();

            foreach (var playerTeam in playerTeamsList[0])
            {
                for (int i = 0; i < playerTeams.Count; i++)
                {
                    if (playerTeams[i].Id == playerTeam.Id)
                    {
                        databaseContext.PlayerTeams.Remove(playerTeams[i]);
                        await databaseContext.SaveChangesAsync();
                        playerTeams.RemoveAt(i);
                        i--;
                    }
                }
            }

            foreach (var playerTeam in playerTeamsList[1])
            {
                if (!playerTeams.Contains(playerTeam))
                {
                    playerTeam.Id = Guid.NewGuid();
                    await databaseContext.PlayerTeams.AddAsync(playerTeam);
                    await databaseContext.SaveChangesAsync();
                }
            }
        }
    }
}
