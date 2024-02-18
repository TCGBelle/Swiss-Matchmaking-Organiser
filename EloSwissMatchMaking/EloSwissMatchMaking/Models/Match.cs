using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EloSwissMatchMaking.Models
{
    public class Match
    {
        private Player _player1;
        public Player Player1 { get { return _player1; } }
        private Player? _player2;
        public Player? Player2 { get { return _player2; } }

        public Match(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void DeclareWinner(int playerX)
        {
           if (playerX == 1)
            {
                if (_player2 != null)
                {
                    _player1.UpdateScoreAndElo(0, _player2.ELO);
                    _player2.UpdateScoreAndElo(2, _player1.ELO);
                }
            }
           else if (playerX == 2)
            {
                if (_player2 != null)
                {
                    _player2.UpdateScoreAndElo(0, _player1.ELO);
                    _player1.UpdateScoreAndElo(2, _player2.ELO);
                }

            }
           else if (playerX == 0)
            {
                //draw
                if (_player2 != null)
                {
                    _player1.UpdateScoreAndElo(1, _player2.ELO);
                    _player2.UpdateScoreAndElo(1, _player1.ELO);
                }
            }
            else
            {
                //throw exception
                MessageBox.Show("I am not updating score", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
