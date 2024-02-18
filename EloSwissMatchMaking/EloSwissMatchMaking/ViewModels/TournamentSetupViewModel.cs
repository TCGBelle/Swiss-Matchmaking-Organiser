using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.Stores;
using EloSwissMatchMaking.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EloSwissMatchMaking.ViewModels
{
    public class TournamentSetupViewModel : ViewModelBase
    {

        private readonly ObservableCollection<PlayerViewModel> _players;
        private readonly Tournament _tournament;

        public ICommand StartTournamentCommand { get; }
        public ICommand AddPlayerManuallyCommand { get; }
        public ICommand AddPlayerfromDBCommand { get; }
        public ICommand RemovePlayerCommand { get; }

        public IEnumerable<PlayerViewModel> Players => _players;

        private PlayerViewModel? _selectedPlayer;

        public PlayerViewModel? SelectedPlayer
        {
            get { return _selectedPlayer; }
            set { _selectedPlayer = value; }
        }

        private string _selectedOption;
        public string SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                if(_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                    OnSelectionChanged();
                }
            }
        }
        public List<string> DropDownOptions { get; } = new List<string> { "Competitive", "Casual" };
        public TournamentSetupViewModel(Tournament tournament, NavigationService startTournamentNavigationService, PopUpService popUpService)
        {
            _players = new ObservableCollection<PlayerViewModel>();
            _selectedPlayer = null;
            StartTournamentCommand = new NavigationCommand(startTournamentNavigationService);
            AddPlayerManuallyCommand = new OpenNewPlayerPopUpCommand(popUpService);
            AddPlayerfromDBCommand = new OpenExistingPlayerPopUpCommand(popUpService);
            RemovePlayerCommand = new RelayCommandBase(execute => RemovePlayer(), canExecute => _selectedPlayer != null);
            _tournament = tournament;
            UpdatePlayerList();
            _tournament.PropertyChanged += OnModelPropertyChanged;
            _selectedOption = "Competitive";
        }

        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdatePlayerList();
        }

        private void UpdatePlayerList()
        {
            _players.Clear();
            foreach (var item in _tournament.GetAllPlayers())
            {
                PlayerViewModel playerViewModel = new PlayerViewModel(item);
                _players.Add(playerViewModel);
            }
        }

        private void RemovePlayer()
        {
            if (_selectedPlayer != null)
            {
                _tournament.RemovePlayer(_selectedPlayer.Id);
            }
        }

        private void OnSelectionChanged()
        {
            if(_selectedOption == "Competitive")
            {
                _tournament.CasualTournament = false;
            }
            else
            {
                _tournament.CasualTournament = true;
            }
        }
    }
}
