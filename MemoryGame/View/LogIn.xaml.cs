using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MemoryGame.ViewModel;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LoginView _LoginView;
        public LogIn()
        {
            InitializeComponent();

            _LoginView = new LoginView(App.SharedMenuCommands);

            DataContext = _LoginView;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = (sender as TextBlock)?.Text;

            if (selectedUser != null)
            {

                _LoginView.MenuCommands.SelectedUserCommand.Execute(selectedUser);
            }
        }
    }
}
