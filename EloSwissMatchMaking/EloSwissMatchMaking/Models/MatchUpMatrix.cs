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
        private List<Player> _players;
        private List<Matchup> _matchups;
        private LinkedList<Match> _optimalMatches;
        public MatchUpMatrix(List<Player> players)
        {
            _players = players;
            _matchups = GenerateAllMatchups();
            _optimalMatches = new LinkedList<Match>();
        }
        public LinkedList<Match> GetBestMatchupCombination()
        {
            List<Matchup> bestCombination = null;
            int minTotalScoreDifference = int.MaxValue;

            // Recursively find the combination of matches with the lowest total score difference
            FindBestCombination(new HashSet<Player>(), new List<Matchup>(), ref bestCombination, ref minTotalScoreDifference);

            //bestCombination;
            foreach (Matchup matchup in bestCombination)
            {
                _optimalMatches.AddLast(CreateNewMatch(matchup.Player1, matchup.Player2));
            }
            return _optimalMatches;
        }

        private void FindBestCombination(HashSet<Player> pairedPlayers, List<Matchup> currentCombination, ref List<Matchup> bestCombination, ref int minTotalScoreDifference)
        {
            // Base case: if all players are paired, calculate the total score difference
            if (pairedPlayers.Count == _players.Count)
            {
                int totalScoreDifference = currentCombination.Sum(m => m.ScoreDifference);
                if (totalScoreDifference < minTotalScoreDifference)
                {
                    minTotalScoreDifference = totalScoreDifference;
                    bestCombination = new List<Matchup>(currentCombination);
                }
                return;
            }

            // Recursive case: try adding each unpaired match to the current combination
            foreach (var matchup in _matchups)
            {
                if (!pairedPlayers.Contains(matchup.Player1) && !pairedPlayers.Contains(matchup.Player2))
                {
                    // Choose
                    currentCombination.Add(matchup);
                    pairedPlayers.Add(matchup.Player1);
                    pairedPlayers.Add(matchup.Player2);

                    // Explore
                    FindBestCombination(pairedPlayers, currentCombination, ref bestCombination, ref minTotalScoreDifference);

                    // Unchoose
                    currentCombination.Remove(matchup);
                    pairedPlayers.Remove(matchup.Player1);
                    pairedPlayers.Remove(matchup.Player2);
                }
            }
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
