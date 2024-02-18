using EloSwissMatchMaking.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class OpenNewPlayerPopUpCommand : CommandBase
    {
        private PopUpService _navigationService;
        public OpenNewPlayerPopUpCommand(PopUpService navService)
        {
            _navigationService = navService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateToNewPlayer();

            //Open Popup
            _navigationService.OpenPopUp();
        }
    }
}
