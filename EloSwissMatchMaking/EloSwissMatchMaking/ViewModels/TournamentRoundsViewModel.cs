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
using System.Windows;
using System.Windows.Input;

namespace EloSwissMatchMaking.ViewModels
{
    public class TournamentRoundsViewModel : ViewModelBase
    {
        private Tournament _tournament;
        private NavigationService _navigationService;
        private readonly ObservableCollection<MatchViewModel> _matchList;
        public IEnumerable<MatchViewModel> MatchList => _matchList;

        private MatchViewModel? _selectedMatch;
        public MatchViewModel? SelectedMatch 
        {
            get { return _selectedMatch; }
            set { _selectedMatch = value; }
        }
        private StringBuilder _roundsText;
        public String RoundsText
        {
            get { return _roundsText.ToString(); }
        }

        public ICommand DropPlayer1Command { get; }
        public ICommand DropPlayer2Command { get; }
        public ICommand NextRoundCommand { get; }
        public ICommand OpenPopUpCommand { get; }
        public ICommand OpenFinalPopUpCommand { get; }

        public TournamentRoundsViewModel(Tournament tournament, NavigationService setUpTournamentNavigationService, PopUpService PopUpNavService)
        {
            _tournament = tournament;
            _navigationService = setUpTournamentNavigationService;
            _matchList = new ObservableCollection<MatchViewModel>();
            _selectedMatch = null;
            DropPlayer1Command = new RelayCommandBase(execute => DropPlayer1(), canExecute => _selectedMatch != null);
            DropPlayer2Command = new RelayCommandBase(execute => DropPlayer2(), canExecute => _selectedMatch != null);
            NextRoundCommand = new RelayCommandBase(execute => NavigateToNextRound(), canExecute => _tournament.CurrentRound < _tournament.TotalRounds);
            OpenPopUpCommand = new OpenStandingsPopUpCommand(PopUpNavService);
            OpenFinalPopUpCommand = new RelayCommandBase(execute => OpenFinalStandingsPopUp(), canExecute => _tournament.CurrentRound == _tournament.TotalRounds);
            _roundsText = new StringBuilder();
            _tournament.StartNewRound();
            _roundsText.Append("Round ");
            _roundsText.Append(_tournament.CurrentRound.ToString());
            _roundsText.Append("/");
            _roundsText.Append(_tournament.TotalRounds.ToString());
            UpdateMatchList();
            _tournament.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateMatchList();
        }

        private void UpdateMatchList()
        {
            _matchList.Clear();
            foreach (var match in _tournament.GetAllMatchs())
            {
                MatchViewModel matchViewModel = new MatchViewModel(match);
                _matchList.Add(matchViewModel);
            }
        }

        private void DropPlayer1()
        {
            if (_selectedMatch != null)
            {
                _tournament.RemovePlayer(_selectedMatch._match.Player1.Id);
            }
        }
        private void DropPlayer2()
        {
            if (_selectedMatch != null)
            {
                if (_selectedMatch._match.Player2 != null)
                {
                    _tournament.RemovePlayer(_selectedMatch._match.Player2.Id);
                }
            }
        }

        private void NavigateToNextRound()
        {
            if(AllMatchesDeclared())
            {
                //navigate
                foreach (MatchViewModel match in _matchList)
                {
                    match.DeclareFinalWinner();
                }
                _navigationService.Navigate();
            }
            else
            {
                //spawn warning pop up
                MessageBox.Show("Not All Matches Have Results", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private bool AllMatchesDeclared()
        {
            foreach (MatchViewModel match in _matchList)
            {
                if(match.WinnerDeclared == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void OpenFinalStandingsPopUp()
        {
            if (AllMatchesDeclared())
            {
                //navigate
                foreach (MatchViewModel match in _matchList)
                {
                    match.DeclareFinalWinner();
                }
                _tournament.UpdatePlayerFinalStandings();
                OpenPopUpCommand.Execute(null);
            }
            else
            {
                //spawn warning pop up
                MessageBox.Show("Not All Matches Have Results", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
