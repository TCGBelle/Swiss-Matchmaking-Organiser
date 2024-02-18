using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class NavigationCommand : CommandBase
    {

        private readonly NavigationService _navigationService;
        public NavigationCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
