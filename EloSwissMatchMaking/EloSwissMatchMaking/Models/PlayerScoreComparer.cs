using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Models
{
    public class PlayerScoreComparer : IComparer<Player>
    {
        public int Compare(Player? x, Player? y)
        {
            int result = 0;
            if (x != null && y != null)
            {
                //compare based on player score
                result = x.Score.CompareTo(y.Score);
                //if the score is the same compare resistance
                if (result == 0)
                {
                    result = x.Resistance.CompareTo(y.Resistance);
                }
            }
            return result;
        }
    }
}
