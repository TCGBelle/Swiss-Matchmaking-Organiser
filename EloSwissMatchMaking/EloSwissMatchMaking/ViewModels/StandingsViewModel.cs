using EloSwissMatchMaking.Models;
using EloSwissMatchMaking.Services.Navigation;
using EloSwissMatchMaking.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Documents;
using System.Windows.Controls;

namespace EloSwissMatchMaking.ViewModels
{
    public class StandingsViewModel : ViewModelBase
    {
        private Tournament _tournament;
        private StringBuilder _standingsStringBuilder;
        private LinkedList<Player> _players;
        public String StandingsString
        { get { return _standingsStringBuilder.ToString(); } }
        public ICommand PrintTextCommand { get; }
        public ICommand ClosePopUpCommand { get; }
        public StandingsViewModel(Tournament tournament, PopUpService navService)
        {
            _tournament = tournament;
            _standingsStringBuilder = new StringBuilder();
            _players = new LinkedList<Player>();
            foreach (var item in _tournament.GetAllPlayers())
            {
                Player player = item;
                _players.AddLast(player);
            }
            for (int x = 0; x < _players.Count; x++)
            {
                _standingsStringBuilder.Append((x+1).ToString());
                _standingsStringBuilder.Append(". ");
                _standingsStringBuilder.Append(_players.ElementAt(x).Id.ToString());
                _standingsStringBuilder.Append(" " + _players.ElementAt(x).Name.ToString());
                _standingsStringBuilder.Append(" " + _players.ElementAt(x).ELO.ToString());
                _standingsStringBuilder.AppendLine();
            }
            PrintTextCommand = new RelayCommandBase(execute => Print(), canExecute => true);
            ClosePopUpCommand = new ClosePopUpCommand(navService);
        }

        private void Print()
        {
            // Create a FlowDocument with the text to be printed
            FlowDocument doc = new FlowDocument(new Paragraph(new Run(StandingsString)));

            PrintDialog printDialog = new PrintDialog();

            if(printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Printing Standings");
            }
        }
    }
}
