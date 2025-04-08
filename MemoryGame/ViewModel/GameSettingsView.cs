using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModel
{
    internal class GameSettingsView
    {
        public MenuCommands MenuCommands { get; set; }

        public GameSettingsView(MenuCommands menuCommands) {
            
            MenuCommands = menuCommands;

        }
    }
}
