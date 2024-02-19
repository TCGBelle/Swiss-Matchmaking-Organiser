using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Models
{
    public class Player
    {
        private string _name;
        public string Name { get { return _name; } }
        private int _id;
        public int Id { get { return _id; } }
        private int _elo;
        public int ELO { get { return _elo; } }
        private int _score;
        public int Score { get { return _score; } }
        private int _resistance;
        public int Resistance { get { return _resistance; } }
        private int _eloDelta;
        private int _eloK = 32; //K is the constant in the elo function can be ajusted to increase or decrease the fluctuations
        private int _scoreForWinning = 3;
        private int _resistanceForBye = 1000;
        private LinkedList<Player> _previousOpponents;
        public LinkedList<Player> PreviousOpponents
        {
            get { return _previousOpponents; }
        }

        public Player(string name, int elo, int id)
        {
            _name = name;
            _elo = elo;
            _id = id;
            _previousOpponents = new LinkedList<Player>();
        }

        public void UpdateScoreAndElo(int win, int OppElo)
        {
            if (win == 0) //player won
            {
                _score += _scoreForWinning;
                _resistance += OppElo;
                //update elo positivley
                _eloDelta = CalculateElo(OppElo, 1.0f);
                _elo += _eloDelta;
            }
            if (win == 1)
            {
                _score++;
                _resistance += OppElo;
                //update elo based on player stats
                _eloDelta = CalculateElo(OppElo, 0.5f);
                _elo += _eloDelta;
            }
            if (win == 2)
            {
                _resistance += OppElo;
                //update elo negativley
                _eloDelta = CalculateElo(OppElo, 0.0f);
                _elo += _eloDelta;
            }
        }

        public void UpdateScoreInBye()
        {
            _score += _scoreForWinning;
            _resistance += _resistanceForBye;
        }

        public void UpdatePreviouslyOpponents(Player Opp)
        {
            _previousOpponents.AddLast(Opp);
        }

        private double ExpectationToWin(int Player1Rating, int Player2Rating)
        {
            double expectationToWin = 1/(1+ Math.Pow(10, (Player2Rating-Player1Rating)/400));
            return expectationToWin;
        }
        private int CalculateElo(int OppElo, float GameOutcome) //game outcome 1 for win 0 for loss
        {
            int delta = (int)(_eloK * (GameOutcome - ExpectationToWin(_elo, OppElo)));
            return delta;
        }
    }
}
