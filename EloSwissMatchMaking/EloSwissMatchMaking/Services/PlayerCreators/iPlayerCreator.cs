using EloSwissMatchMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Services.PlayerCreators
{
    public interface iPlayerCreator
    {
        Task CreatePlayer(Player player);
    }
}
