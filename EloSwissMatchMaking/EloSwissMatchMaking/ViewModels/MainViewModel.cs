using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrViewModel => _navigationStore.CurrViewModel;

        public MainViewModel(Tournament tournament, NavigationStore navStore) 
        {
            _navigationStore = navStore;
            _navigationStore.CurrViewModelChange += OnCurrViewModelChanged;
        }

        private void OnCurrViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrViewModel));
        }
    }
}
