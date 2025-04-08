using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModel
{
    public class MainWindowView
    {
        public MenuCommands MenuCommands { get; set; }

        public ObservableCollection<TileView> Tiles { get; set; }

        public int GridRows { get; set; }
        public int GridColumns { get; set; }



        public MainWindowView(MenuCommands menuCommands)
        {
            MenuCommands = menuCommands;

            Tiles = new ObservableCollection<TileView>();

            App.SharedMenuCommands.GameTiles = Tiles;
        }

    }
}
