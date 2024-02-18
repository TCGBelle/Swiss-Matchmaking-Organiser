using EloSwissMatchMaking.Stores;
using EloSwissMatchMaking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EloSwissMatchMaking.Services.Navigation
{
    public class PopUpService
    {
        private Window _popUpWindow;
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _createNewPlayerViewModel;
        private readonly Func<ViewModelBase> _createExistingPlayerViewModel;
        private readonly Func<ViewModelBase> _createStandingsViewModel;

        public PopUpService(Window popUpWindow, NavigationStore navStore, Func<ViewModelBase> createNewPlayerViewModel, Func<ViewModelBase> createExistingPlayerViewModel, Func<ViewModelBase> createStandingsViewModel)
        {
            _popUpWindow = popUpWindow;
            _navigationStore = navStore;
            _createNewPlayerViewModel = createNewPlayerViewModel;
            _createExistingPlayerViewModel = createExistingPlayerViewModel;
            _createStandingsViewModel = createStandingsViewModel;
        }

        public void OpenPopUp()
        {
            _popUpWindow.Show();
        }
        public void ClosePopUp()
        {
            _popUpWindow.Hide();
        }

        public void NavigateToExistingPlayer()
        {
            _navigationStore.CurrViewModel = _createExistingPlayerViewModel();
        }

        public void NavigateToNewPlayer()
        {
            _navigationStore.CurrViewModel = _createNewPlayerViewModel();
        }

        public void NavigateToStandings()
        {
            _navigationStore.CurrViewModel = _createStandingsViewModel();
        }
        
        public ViewModelBase CurrentView()
        {
            return _navigationStore.CurrViewModel;
        }
    }
}
