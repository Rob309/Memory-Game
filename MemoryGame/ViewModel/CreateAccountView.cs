using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class CreateAccountView
    {
        public MenuCommands MenuCommands { get; set; }

        public CreateAccountView(MenuCommands menuCommands) 
        {
            MenuCommands = menuCommands;
        }

    }
}
