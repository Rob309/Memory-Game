using MemoryGame.Memory;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MemoryGame.ViewModel
{
    public class LoginView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MenuCommands MenuCommands { get; set; }
        public ObservableCollection<string> Users { get; set; } = new ObservableCollection<string>();

        private ImageBrush _characterImage;
        public ImageBrush CharacterImage
        {
            get => _characterImage;
            set
            {
                _characterImage = value;
                OnPropertyChanged(nameof(CharacterImage));
            }
        }

        public LoginView(MenuCommands menuCommands)
        {
            MenuCommands = menuCommands;
            Users = new ObservableCollection<string>(SaveManager.Instance.LoadFolders());
            MenuCommands.SetUsers(Users);

            //CharacterImage = new ImageBrush(App.images[0]);
            MenuCommands.OnChangeSelected += MenuCommands_OnChangeSelected;
        }

        private void MenuCommands_OnChangeSelected(object sender, EventArgs e)
        {
            CharacterImage = new ImageBrush(App.images[App.userData.imageID]);

        }

        ~LoginView() { MenuCommands.OnChangeSelected -= MenuCommands_OnChangeSelected; }
    }
}
