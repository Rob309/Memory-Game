using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MemoryGame.ViewModel;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public string inputText { get; private set; }
        public CreateAccount()
        {
            InitializeComponent();

            DataContext = new CreateAccountView(App.SharedMenuCommands);
            ImageBrush brush = new ImageBrush(App.images[0]); 
            CharacterFrame.Background = brush;
        }
    }
}
