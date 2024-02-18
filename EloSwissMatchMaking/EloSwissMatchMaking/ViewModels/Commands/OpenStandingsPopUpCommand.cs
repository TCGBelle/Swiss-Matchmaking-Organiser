using EloSwissMatchMaking.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class OpenStandingsPopUpCommand : CommandBase
    {
        private PopUpService _navigationService;
        public OpenStandingsPopUpCommand(PopUpService navService)
        {
            _navigationService = navService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.NavigateToStandings();
            _navigationService.OpenPopUp();
        }
    }
}
