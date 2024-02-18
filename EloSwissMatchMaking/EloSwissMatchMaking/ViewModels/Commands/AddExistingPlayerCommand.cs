using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels.Commands
{
    public class AddExistingPlayerCommand : CommandBase
    {
        private readonly AddPlayerbyDbViewModel _parentViewModel;
        private readonly Tournament _tournament;
        private readonly PopUpService _navService;

        public AddExistingPlayerCommand(Tournament tournament, AddPlayerbyDbViewModel parentVM, PopUpService navService)
        {
            _parentViewModel = parentVM;
            _tournament = tournament;
            _navService = navService;

            _parentViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AddPlayerbyDbViewModel.PlayerId))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_parentViewModel.PlayerId))
            {
                if (IsDigitsOnly(_parentViewModel.PlayerId))
                {
                    return base.CanExecute(parameter);
                }
            }
            return false;
        }
        private bool IsDigitsOnly(string str)
        {
            if(str.Length != 8)
            {
                return false;
            }
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }
        public override void Execute(object? parameter)
        {
            
            try
            {
                int tempID = Int32.Parse(_parentViewModel.PlayerId);
                _tournament.AddExistingPlayer(tempID);
                //set up tournament view model update player list... subscribe to event?
                _navService.ClosePopUp();
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{_parentViewModel.PlayerId}'");
            }

        }
    }
}
