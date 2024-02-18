using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class AddNewPlayerCommand : CommandBase
    {
        private readonly AddPlayerManuallyViewModel _parentViewModel;
        private readonly Tournament _tournament;
        private readonly PopUpService _navService;

        public AddNewPlayerCommand(Tournament tournament, AddPlayerManuallyViewModel viewModel, PopUpService navService)
        {
            _parentViewModel = viewModel;
            _tournament = tournament;
            _navService = navService;

            _parentViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(AddPlayerManuallyViewModel.Username))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_parentViewModel.Username) && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _tournament.AddNewPlayer(_parentViewModel.Username);
            _navService.ClosePopUp();
        }
    }
}
