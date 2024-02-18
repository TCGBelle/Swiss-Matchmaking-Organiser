using EloSwissMatchMaking.Services.PlayerCreators;
using EloSwissMatchMaking.Services.PlayerProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EloSwissMatchMaking.Models
{
#pragma warning disable CS8604
    public class Tournament : INotifyPropertyChanged
    {
        private readonly DatabasePlayerProvider _playerProvider;
        private readonly DatabasePlayerCreator _playerCreator;
        private LinkedList<Player> _playerList;
        private LinkedList<Player> _removableCopyPlayerList; //intalized here do avoid creating/passing a potentially large list multiple times in pairroundx func
        private LinkedList<Match> _currentMatches; // both _playerList and _currentMatches can be null but there are more warnings if i make them nullable
        private int _currentRound;
        public int CurrentRound { get { return _currentRound; } }
        private int _totalRounds;
        public int TotalRounds { get { return _totalRounds; } }
        private bool _casualTournament;
        public bool CasualTournament
        {
            get { return _casualTournament; }
            set { _casualTournament = value;}
        }
        private static readonly Random _random = new Random();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Tournament(DatabasePlayerProvider playerProvider, DatabasePlayerCreator playerCreator)
        {
            _playerProvider = playerProvider;
            _playerCreator = playerCreator;
            _currentRound = 0;
            _casualTournament= false;
            _playerList = new LinkedList<Player>();
            _removableCopyPlayerList = new LinkedList<Player>();
            _currentMatches = new LinkedList<Match>();
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*public void HardCodePlayers() //testing function
        {
            Player Player1 = new Player("Annabelle McQuade", 1500, 11111111);
            _playerList.AddFirst(Player1);
            Player1 = new Player("Galin Yornadov", 1259, 11111116);
            _playerList.AddFirst(Player1);
            Player1 = new Player("Arran Anderson", 952, 11111120);
            _playerList.AddFirst(Player1);
            Player1 = new Player("Gaelin Yornadov", 1258, 11111126);
            _playerList.AddFirst(Player1);
            Player1 = new Player("Declan Patrick", 1450, 11111129);
            _playerList.AddFirst(Player1);
        }*/ //function no longer required as players can be loaded from Database
        public void AddNewPlayer(string  playerName)
        {
            int previousHighestID = _playerProvider.ReturnHighestID();
           
            foreach (var player in _playerList)
            {
                if (player.Id > previousHighestID)
                {
                    previousHighestID = player.Id;
                }
            }
            //add random value to ID
            int newID = previousHighestID + _random.Next(1, 10);
            _playerList.AddLast(new Player (playerName, 800, newID));
            OnPropertyChanged(nameof(_playerList));
        }
        public void AddExistingPlayer(int  playerID)
        {
            if(_playerProvider.ReturnSpecifiedPlayer(playerID) != null)
            {
                MessageBox.Show("Found Player" + playerID.ToString(), "Success", MessageBoxButton.OK, MessageBoxImage.Warning);
                _playerList.AddLast(_playerProvider.ReturnSpecifiedPlayer(playerID));
                OnPropertyChanged(nameof(_playerList));
            }
        }

        public void RemovePlayer(int playerID)
        {
            for (int x = 0;  x < _playerList.Count; x++)
            {
                if (_playerList.ElementAt(x).Id == playerID)
                {
                    _playerList.Remove(_playerList.ElementAt(x));
                    OnPropertyChanged(nameof(_playerList));
                    break;
                }
            }
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _playerList;
        }

        public IEnumerable<Match> GetAllMatchs()
        {
            return _currentMatches;
        }

        private void SortListByElo()
        {
            //sorted by elo for round 1 pairing
            _playerList = new LinkedList<Player>(_playerList.OrderByDescending(Player => Player.ELO));
            
        }
        private void SortListByScore()
        {
            _playerList = new LinkedList<Player>(_playerList.OrderByDescending(Player => Player, new PlayerScoreComparer()));
        }
        private bool WillThereBeABye()
        {
            //if we have an odd number of players, the highest elo player gets a bye(a free win) round 1
            //in round 1+x the player with lowest score by highest resitance gets free win if odd number of players
            if (_playerList.Count % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void NumberOfRounds()
        {
            double noOfRounds = Math.Log2(_playerList.Count);
            noOfRounds = Math.Ceiling(noOfRounds);
            _totalRounds = (int)noOfRounds;
        }
        public void StartNewRound()
        {
            _currentRound++;
            if(_currentRound == 1)
            {
                if (CasualTournament == true)
                {
                    PairRound1Casually();
                }
                else
                {
                    PairRound1();
                }
            }
            else
            {
                PairRoundX();
            }
        }

        private void PairRound1Casually()
        {
            NumberOfRounds();
            SortListByElo();
            _removableCopyPlayerList.Clear();
            foreach (Player player in _playerList)
            {
                _removableCopyPlayerList.AddLast(player);
            }
            if (WillThereBeABye())
            {
                int rando = _random.Next(0, _removableCopyPlayerList.Count);
                _currentMatches.AddLast(CreateNewMatch(_removableCopyPlayerList.ElementAt(rando), null));
                _removableCopyPlayerList.Remove(_removableCopyPlayerList.ElementAt(rando));
            }
            int x;
            int y;
            while (_removableCopyPlayerList.Count != 0)
            {
                x = 0;
                y = 0;
                while(x==y)
                {
                    x = _random.Next(0, _removableCopyPlayerList.Count);
                    y = _random.Next(0, _removableCopyPlayerList.Count);
                }
                _currentMatches.AddLast(CreateNewMatch(_removableCopyPlayerList.ElementAt(x), _removableCopyPlayerList.ElementAt(y)));
                _removableCopyPlayerList.Remove(_removableCopyPlayerList.ElementAt(x));
                _removableCopyPlayerList.Remove(_removableCopyPlayerList.ElementAt(y));
            }
        }
        private void PairRound1()
        {
            int HeadPointer = 0;
            int TailPointer = _playerList.Count() - 1;
            NumberOfRounds();
            SortListByElo();
            if(WillThereBeABye())
            {
                _currentMatches.AddLast(CreateNewMatch(_playerList.First(), null));
                HeadPointer++;
            }
            while(HeadPointer < TailPointer)
            {
                //pair the highest elo player with the lowest elo player for round 1
                _currentMatches.AddLast(CreateNewMatch(_playerList.ElementAt(HeadPointer), _playerList.ElementAt(TailPointer)));
                HeadPointer++;
                TailPointer--;
            }
        }
        private void PairRoundX()
        {
            _currentMatches.Clear();
            Player inspectedPlayer;
            Player nextInspectedPlayer;
            SortListByScore();
            _removableCopyPlayerList.Clear();
            foreach(Player player in _playerList)
            {
                _removableCopyPlayerList.AddLast(player);
            }
            //_removableCopyPlayerList = _playerList;
            if (WillThereBeABye())
            {
                //if there is a bye get the player with the worst score but highest resistance and give them the bye
                int x = _removableCopyPlayerList.Count()-1;
                bool foundByeCandate = false;
                while(foundByeCandate == false && x >= _removableCopyPlayerList.Count()/2)
                {
                    inspectedPlayer = _removableCopyPlayerList.ElementAt(x);
                    nextInspectedPlayer = _removableCopyPlayerList.ElementAt(x-1);
                    if(inspectedPlayer.Score != nextInspectedPlayer.Score)
                    {
                        //since the list is sorted if the next player has a diffrent score it means we have found the highest rated player of the lowest score
                        _currentMatches.AddLast(CreateNewMatch(_removableCopyPlayerList.ElementAt(x), null));
                        _removableCopyPlayerList.Remove(_removableCopyPlayerList.ElementAt(x));
                        foundByeCandate = true;
                    }
                    x--;
                }
            }

            MatchUpMatrix matchUpMatrix = new MatchUpMatrix();
            LinkedList<Match> matches = matchUpMatrix.FindOptimalMatches(_removableCopyPlayerList);
            // a combination of Gale-Shapleys Stable Marriage Algorithim, with a score minimizing system to ensure pairings are optimal and no repeats. understand it is slow but only way i could get it to work
            foreach (Match match in matches)
            {
                _currentMatches.AddLast(match);
            }
          
        }

        public void UpdatePlayerFinalStandings()
        {
            SortListByScore();
            foreach (Player player in _playerList)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _playerCreator.UpdatePlayerDB(player);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private Match CreateNewMatch(Player player1, Player? player2)
        {
            Match tempMatch = new Match(player1, player2); //know its complaining about it being possibly null but need to check if its null for byes
            return tempMatch;
        }
    }

}
