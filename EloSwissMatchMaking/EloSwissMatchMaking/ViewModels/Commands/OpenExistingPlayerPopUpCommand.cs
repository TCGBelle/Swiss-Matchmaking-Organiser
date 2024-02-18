using EloSwissMatchMaking.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class OpenExistingPlayerPopUpCommand : CommandBase
    {
        private PopUpService _navigationService;
        public OpenExistingPlayerPopUpCommand(PopUpService popUpService)
        {
            _navigationService = popUpService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateToExistingPlayer();

            //Open Popup
            _navigationService.OpenPopUp();
        }
    }
}
