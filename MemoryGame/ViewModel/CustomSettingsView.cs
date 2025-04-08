using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ViewModel
{
    public class CustomSettingsView
    {
        public MenuCommands MenuCommands { get; set; }
        public CustomSettingsView(MenuCommands menuCommands)
        {
            MenuCommands = menuCommands;

        }

    }
}
