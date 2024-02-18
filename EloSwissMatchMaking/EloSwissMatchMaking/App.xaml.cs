using EloSwissMatchMaking.DBContext;
using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.Services.PlayerCreators;
using EloSwissMatchMaking.Services.PlayerProviders;
using EloSwissMatchMaking.Stores;
using EloSwissMatchMaking.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EloSwissMatchMaking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Tournament _tournament;
        private readonly NavigationStore _navigationStore;
        private readonly NavigationStore _popUpNavStore;
        private const string CONNECTION_STRING = "Data Source=ELO.db";
        private readonly DBContextFactory _dbContextFactory;
        private PopUpWindow popUpWindow;
        public App()
        {
            _dbContextFactory = new DBContextFactory(CONNECTION_STRING);
            DatabasePlayerProvider playerProvider = new DatabasePlayerProvider(_dbContextFactory);
            DatabasePlayerCreator playerCreator = new DatabasePlayerCreator(_dbContextFactory);
            _tournament = new Tournament(playerProvider, playerCreator);
            _navigationStore = new NavigationStore();
            _popUpNavStore = new NavigationStore();
            popUpWindow = new PopUpWindow()
            {
                DataContext = new PopUpViewModel(_tournament, _popUpNavStore)
            };
        }
        private TournamentRoundsViewModel CreateTournamentRoundsViewModel()
        {
            return new TournamentRoundsViewModel(_tournament, new NavigationService(_navigationStore, CreateTournamentRoundsViewModel), new PopUpService(popUpWindow, _popUpNavStore, CreateAddPlayerManuallyViewModel, CreateAddPlayerbyDBViewModel, CreateStandingsViewModel));
        }

        private TournamentSetupViewModel CreateTournamentSetupViewModel()
        {
            return new TournamentSetupViewModel(_tournament, new NavigationService(_navigationStore, CreateTournamentRoundsViewModel), new PopUpService(popUpWindow, _popUpNavStore, CreateAddPlayerManuallyViewModel, CreateAddPlayerbyDBViewModel, CreateStandingsViewModel));
        }

        private AddPlayerManuallyViewModel CreateAddPlayerManuallyViewModel()
        {
            return new AddPlayerManuallyViewModel(_tournament, new PopUpService(popUpWindow, _popUpNavStore, CreateAddPlayerManuallyViewModel, CreateAddPlayerbyDBViewModel, CreateStandingsViewModel));
        }

        private AddPlayerbyDbViewModel CreateAddPlayerbyDBViewModel()
        {
            return new AddPlayerbyDbViewModel(_tournament, new PopUpService(popUpWindow, _popUpNavStore, CreateAddPlayerManuallyViewModel, CreateAddPlayerbyDBViewModel, CreateStandingsViewModel));
        }
        private StandingsViewModel CreateStandingsViewModel()
        {
            return new StandingsViewModel(_tournament, new PopUpService(popUpWindow, _popUpNavStore, CreateAddPlayerManuallyViewModel, CreateAddPlayerbyDBViewModel, CreateStandingsViewModel));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (PlayerDBContext dBContext = _dbContextFactory.CreateDbContext())
            {
                dBContext.Database.Migrate();
            }
            _navigationStore.CurrViewModel = CreateTournamentSetupViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_tournament, _navigationStore)
            };
            MainWindow.Show();
            popUpWindow.Hide();

            base.OnStartup(e);
        }
    }
}
