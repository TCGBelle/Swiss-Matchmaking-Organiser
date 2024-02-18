using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels
{
    public class PopUpViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrViewModel => _navigationStore.CurrViewModel;

        public PopUpViewModel (Tournament tournament, NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrViewModelChange += OnCurrViewModelChanged;
        }

        private void OnCurrViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrViewModel));
        }
    }
}
