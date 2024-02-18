using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Models
{
#pragma warning disable CS8604
    public struct Matchup
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int ScoreDifference { get; set; }
    }
    public class MatchUpMatrix
    {
        private List<Player> _players = new List<Player>();
        private LinkedList<Match> _optimalMatches = new LinkedList<Match>();
        public LinkedList<Match> FindOptimalMatches(LinkedList<Player> players)
        {
            _players = players.ToList();
            _optimalMatches.Clear();
            List<Matchup> matchups = GenerateAllMatchups();
            HashSet<Player> pairedPlayers = new HashSet<Player>();
            List<Matchup> optimalMatchups = new List<Matchup>();

            // Keep pairing players until all players are paired
            while (pairedPlayers.Count < players.Count)
            {
                var bestMatch = GetBestMatch(matchups, pairedPlayers);
                if (bestMatch.Equals(default(Matchup)))
                {
                    // No possible match found, break out of loop
                    break;
                }
                optimalMatchups.Add(bestMatch);
                pairedPlayers.Add(bestMatch.Player1);
                pairedPlayers.Add(bestMatch.Player2);
            }
            foreach (Matchup matchup in optimalMatchups)
            {
                _optimalMatches.AddLast(CreateNewMatch(matchup.Player1, matchup.Player2));
            }
            return _optimalMatches;
        }
        private Matchup GetBestMatch(List<Matchup> matchups, HashSet<Player> pairedPlayers)
        {
            var bestMatch = matchups.FirstOrDefault(m => !pairedPlayers.Contains(m.Player1) && !pairedPlayers.Contains(m.Player2));
            if (bestMatch.Equals(default(Matchup)))
                return default(Matchup);

            foreach (var match in matchups.Where(m => !pairedPlayers.Contains(m.Player1) && !pairedPlayers.Contains(m.Player2)))
            {
                if (match.ScoreDifference < bestMatch.ScoreDifference)
                    bestMatch = match;
            }

            return bestMatch;
        }
        private List<Matchup> GenerateAllMatchups()
        {
            var matchups = new List<Matchup>();

            for (int i = 0; i < _players.Count; i++)
            {
                for (int j = i + 1; j < _players.Count; j++)
                {
                    if (_players[i] != _players[j])
                    {
                        int scoreDifference = Math.Abs(_players[i].Score - _players[j].Score);
                        foreach (Player p in _players[i].PreviousOpponents)
                        {
                            if (p == _players[j])
                            {
                                scoreDifference += 1000;//add a butt tone of points to matches previously played by same people to discourage them unless absalutley nessasary
                                break; // Exit the loop once a previous opponent is found
                            }
                        }
                        matchups.Add(new Matchup { Player1 = _players[i], Player2 = _players[j], ScoreDifference = scoreDifference });
                    }
                }
            }

            // Sort matchups by score difference in ascending order
            matchups.Sort((m1, m2) => m1.ScoreDifference.CompareTo(m2.ScoreDifference));

            return matchups;
        }
        private Match CreateNewMatch(Player player1, Player? player2)
        {
            Match tempMatch = new Match(player1, player2); //know its complaining about it being possibly null but need to check if its null for byes
            return tempMatch;
        }
    }
}
