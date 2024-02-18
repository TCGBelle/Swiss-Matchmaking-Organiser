using EloSwissMatchMaking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.Stores
{
    public class NavigationStore
    {

        private ViewModelBase? _currViewModel;
        public ViewModelBase? CurrViewModel
        {
            get => _currViewModel;
            set
            {
                _currViewModel = value;
                OnCurrViewModelChanged();
            }
        }

        public event Action? CurrViewModelChange;
        private void OnCurrViewModelChanged()
        {
            CurrViewModelChange?.Invoke();
        }

    }
}
