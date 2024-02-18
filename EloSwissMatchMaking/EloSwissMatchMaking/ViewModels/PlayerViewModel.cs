using EloSwissMatchMaking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels
{
    public class PlayerViewModel: ViewModelBase
    {
        private readonly Player _player;

        public string Name => _player.Name;
        public int Id => _player.Id;
        public int Elo => _player.ELO;

        public PlayerViewModel(Player player)
        {
            _player = player;
        }
    }
}
