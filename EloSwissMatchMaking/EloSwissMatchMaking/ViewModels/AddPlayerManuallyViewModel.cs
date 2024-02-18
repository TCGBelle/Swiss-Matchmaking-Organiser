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
    public class AddPlayerManuallyViewModel : ViewModelBase
    {
        public ICommand AddNewPlayersCommand { get; }
        public ICommand CancelCommand { get; }

        private string _userName;
        public string Username
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public AddPlayerManuallyViewModel(Tournament tournament, PopUpService navService)
        {
            _userName = "";
            AddNewPlayersCommand = new AddNewPlayerCommand(tournament, this, navService);
            CancelCommand = new ClosePopUpCommand(navService);
        }
    }
}
