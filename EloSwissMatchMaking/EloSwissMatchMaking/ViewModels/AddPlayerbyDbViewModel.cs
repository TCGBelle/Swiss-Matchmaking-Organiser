using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EloSwissMatchMaking.ViewModels
{
    public class AddPlayerbyDbViewModel : ViewModelBase
    {
        public ICommand AddPlayerFromDBCommand { get; }
        public ICommand CancelCommand { get; }

        private string _playerId;

        public string PlayerId
        {
            get { return _playerId; }
            set
            {
                _playerId = value;
                OnPropertyChanged(nameof(PlayerId));
            }
        }

        public AddPlayerbyDbViewModel(Tournament tournament, PopUpService navService)
        {
            _playerId = "";
            AddPlayerFromDBCommand = new AddExistingPlayerCommand(tournament, this, navService);
            CancelCommand = new ClosePopUpCommand(navService);
        }
    }
}
