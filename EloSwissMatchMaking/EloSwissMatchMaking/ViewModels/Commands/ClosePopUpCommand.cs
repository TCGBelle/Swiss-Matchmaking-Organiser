using EloSwissMatchMaking.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class ClosePopUpCommand : CommandBase
    {
        private readonly PopUpService _navigationService;
        public ClosePopUpCommand(PopUpService navService)
        {
            _navigationService = navService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.ClosePopUp();
        }
    }
}
