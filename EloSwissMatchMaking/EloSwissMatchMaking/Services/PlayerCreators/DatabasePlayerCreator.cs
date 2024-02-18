using EloSwissMatchMaking.DBContext;
using EloSwissMatchMaking.DBContext.DTO;
using EloSwissMatchMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Services.PlayerCreators
{
    public class DatabasePlayerCreator : iPlayerCreator
    {
        private readonly DBContextFactory _dBContextFactory;

        public DatabasePlayerCreator(DBContextFactory dBContextFactory)
        {
            _dBContextFactory = dBContextFactory;
        }

        public async Task CreatePlayer(Player player)
        {
            using (PlayerDBContext context = _dBContextFactory.CreateDbContext())
            {
                PlayerDTO playerDTO = ToPlayerDTO(player);

                context.Players.Add(playerDTO);
                Console.WriteLine($"{playerDTO.Name} Created");
                await context.SaveChangesAsync();
            }
        }

        private PlayerDTO ToPlayerDTO(Player player)
        {
            return new PlayerDTO()
            {
                Id = player.Id, ELO = player.ELO, Name = player.Name
            };

        }

        public async Task UpdatePlayerDB(Player player)
        {
            using (PlayerDBContext context = _dBContextFactory.CreateDbContext())
            {
                var existingPlayer = context.Players.FirstOrDefault(p => p.Id == player.Id);
                if (existingPlayer != null)
                {
                    existingPlayer.Name = player.Name;
                    existingPlayer.ELO = player.ELO;
                    existingPlayer.Id = player.Id;
                    Console.WriteLine($" {existingPlayer.Name} Updated");
                }
                else
                {
                    await CreatePlayer(player);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
