using EloSwissMatchMaking.DBContext;
using EloSwissMatchMaking.DBContext.DTO;
using EloSwissMatchMaking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EloSwissMatchMaking.Services.PlayerProviders
{
    public class DatabasePlayerProvider : IPlayerProvider
    {
        private readonly DBContextFactory _dBContextFactory;

        public DatabasePlayerProvider(DBContextFactory dBContextFactory)
        {
            _dBContextFactory = dBContextFactory;
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            using (PlayerDBContext context = _dBContextFactory.CreateDbContext())
            {
                IEnumerable<PlayerDTO> playerDTOs = await context.Players.ToListAsync();

                return playerDTOs.Select(r => new Player(r.Name, r.ELO, r.Id));
            }
        }

        public Player? ReturnSpecifiedPlayer(int id)
        {
            using (PlayerDBContext context = _dBContextFactory.CreateDbContext())
            {
                PlayerDTO? playerByID = context.Players.FirstOrDefault(p => p.Id == id);
                if (playerByID != null)
                {
                    return new Player(playerByID.Name, playerByID.ELO, playerByID.Id);
                }
                else
                {
                    MessageBox.Show("Cant Find Player" +id.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                return null;
            }
        }
        public int ReturnHighestID()
        {
            using (PlayerDBContext context = _dBContextFactory.CreateDbContext())
            {
                int highestID = context.Players.Max(p => (int?)p.Id)??0;
                return highestID;
            }
        }
    }
}
