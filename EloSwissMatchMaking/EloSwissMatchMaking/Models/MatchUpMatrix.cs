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
    public static class MatchUpMatrix
    {
        /*public MatchUpMatrix(List<Player> players)
        {
            _players = players;
            _matchups = GenerateAllMatchups();
            _optimalMatches = new LinkedList<Match>();
        }*/
        public static LinkedList<Match> GetBestMatchupCombination(ref List<Player> PlayerListRef)
        {
            List<Matchup> allMatchUps = GenerateAllMatchups(ref PlayerListRef);
            List<Matchup> bestCombination = null;
            LinkedList<Match> optimalMatches = new LinkedList<Match>();
            int minTotalScoreDifference = int.MaxValue;


            // Recursively find the combination of matches with the lowest total score difference
            FindBestCombination(new HashSet<Player>(), new List<Matchup>(), ref bestCombination, ref minTotalScoreDifference, ref PlayerListRef, ref allMatchUps);

            //bestCombination;
            foreach (Matchup matchup in bestCombination)
            {
                optimalMatches.AddLast(CreateNewMatch(matchup.Player1, matchup.Player2));
            }
            return optimalMatches;
        }

        private static void FindBestCombination(HashSet<Player> pairedPlayers, List<Matchup> currentCombination, ref List<Matchup> bestCombination, ref int minTotalScoreDifference, ref List<Player> PlayerListRef, ref List<Matchup> allMatchUpsRef)
        {
            // Base case: if all players are paired, calculate the total score difference
            if (pairedPlayers.Count == PlayerListRef.Count)
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
            foreach (var matchup in allMatchUpsRef)
            {
                if (!pairedPlayers.Contains(matchup.Player1) && !pairedPlayers.Contains(matchup.Player2))
                {
                    // Choose
                    currentCombination.Add(matchup);
                    pairedPlayers.Add(matchup.Player1);
                    pairedPlayers.Add(matchup.Player2);

                    // Explore
                    FindBestCombination(pairedPlayers, currentCombination, ref bestCombination, ref minTotalScoreDifference, ref PlayerListRef, ref allMatchUpsRef);

                    // Unchoose
                    currentCombination.Remove(matchup);
                    pairedPlayers.Remove(matchup.Player1);
                    pairedPlayers.Remove(matchup.Player2);
                }
            }
        }
        private static List<Matchup> GenerateAllMatchups(ref List<Player> PlayerListRef)
        {
            var matchups = new List<Matchup>();

            for (int i = 0; i < PlayerListRef.Count; i++)
            {
                for (int j = i + 1; j < PlayerListRef.Count; j++)
                {
                    if (PlayerListRef[i] != PlayerListRef[j])
                    {
                        int scoreDifference = Math.Abs(PlayerListRef[i].Score - PlayerListRef[j].Score);
                        foreach (Player p in PlayerListRef[i].PreviousOpponents)
                        {
                            if (p == PlayerListRef[j])
                            {
                                scoreDifference += 1000;//add a butt tone of points to matches previously played by same people to discourage them unless absalutley nessasary
                            }
                        }
                        matchups.Add(new Matchup { Player1 = PlayerListRef[i], Player2 = PlayerListRef[j], ScoreDifference = scoreDifference });
                    }
                }
            }

            // Sort matchups by score difference in ascending order
            matchups.Sort((m1, m2) => m1.ScoreDifference.CompareTo(m2.ScoreDifference));

            return matchups;
        }
        private static Match CreateNewMatch(Player player1, Player? player2)
        {
            Match tempMatch = new Match(player1, player2); //know its complaining about it being possibly null but need to check if its null for byes
            return tempMatch;
        }
    }
}
