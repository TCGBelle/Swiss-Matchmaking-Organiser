using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EloSwissMatchMaking.ViewModels
{
    public class MatchViewModel : ViewModelBase
    {
        public Match _match;
        public bool _winnerDeclared;
        private StringBuilder _player1Info;
        private StringBuilder _player2Info;
        private int _provinsoalWinner;

        public String Player1Info => _player1Info.ToString();
        public String Player2Info => _player2Info.ToString();
        private SolidColorBrush _player1Brush;
        public SolidColorBrush Player1Brush
        {
            get => _player1Brush;
            set
            {
                _player1Brush = value;
                OnPropertyChanged(nameof(Player1Brush));
            }
        }
        private SolidColorBrush _player2Brush;
        public SolidColorBrush Player2Brush
        {
            get => _player2Brush;
            set
            {
                _player2Brush = value;
                OnPropertyChanged(nameof(Player2Brush));
            }
        }

        public ICommand Player1WinCommand { get; }
        public ICommand Player2WinCommand { get; }
        public ICommand DrawCommand { get; }
        
        public MatchViewModel(Match match)
        {
            _match = match;
            _player1Info = new StringBuilder();
            _player2Info = new StringBuilder();
            _player1Info.AppendLine(_match.Player1.Id.ToString());
            _player1Info.AppendLine(_match.Player1.Name);
            _player1Brush = Brushes.White;
            _player2Brush = Brushes.White;
            if (_match.Player2 == null)
            {
                _player2Info.AppendLine("Bye");
                Player1ProvincalWin();
                Player1Brush = Brushes.Green;
                Player2Brush = Brushes.Red;
            }
            else
            {
                _player2Info.AppendLine(_match.Player1.Id.ToString());
                _player2Info.AppendLine(_match.Player2.Name);
            }
            _provinsoalWinner = 0;
            Player1WinCommand = new RelayCommandBase(execute => Player1ProvincalWin(), canExecute => true);
            Player2WinCommand = new RelayCommandBase(execute => Player2ProvincalWin(), canExecute => _match.Player2 != null);
            DrawCommand = new RelayCommandBase(execute => ProvincalDraw(), canExecute => _match.Player2 != null);
            _winnerDeclared = false;
        }

        public void Player1ProvincalWin()
        {
            _provinsoalWinner = 1;
            Player1Brush = Brushes.Green;
            Player2Brush = Brushes.Red;
            _winnerDeclared = true;
        }
        public void Player2ProvincalWin()
        {
            _provinsoalWinner = 2;
            Player1Brush = Brushes.Red;
            Player2Brush = Brushes.Green;
            _winnerDeclared = true;
        }
        public void ProvincalDraw()
        {
            _provinsoalWinner = 0;
            Player1Brush = Brushes.Gray;
            Player2Brush = Brushes.Gray;
            _winnerDeclared = true;
        }
        public void DeclareFinalWinner()
        {
            //is called when round ends to lock in choice
            _match.DeclareWinner(_provinsoalWinner);
        }
    }
}
