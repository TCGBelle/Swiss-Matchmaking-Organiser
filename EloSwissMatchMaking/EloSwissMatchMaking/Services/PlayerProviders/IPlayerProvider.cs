using EloSwissMatchMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Services.PlayerProviders
{
    public interface IPlayerProvider
    {
        Task<IEnumerable<Player>> GetAllPlayers();
    }
}
