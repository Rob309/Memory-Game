using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MemoryGame.Memory;
using MemoryGame.ViewModel;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowView _mainWindow;
        public DispatcherTimer gameTimer;
        private TimeSpan remainingTime;
        public bool isGameActive = false;
        public MainWindow()
        {
            InitializeComponent();

            _mainWindow = new MainWindowView(App.SharedMenuCommands);

            this.DataContext = _mainWindow;

            ImageBrush brush = new ImageBrush(App.images[App.userData.imageID]);
            UserFrame.Background = brush;

            Board.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;

            App.SharedMenuCommands.OnGameStarted += SharedMenuCommands_OnGameStarted;
            App.SharedMenuCommands.OnWin += SharedMenuCommands_OnWin;
            App.SharedMenuCommands.OnGameLoaded += SharedMenuCommands_OnGameLoaded;
        }

        private void SharedMenuCommands_OnGameLoaded(object sender, EventArgs e)
        {
            remainingTime = TimeSpan.FromSeconds(App.userData.gameData.timeLeft);
            StartGameTimer();
        }

        private void SharedMenuCommands_OnWin(object sender, EventArgs e)
        {
            isGameActive = false;
            gameTimer.Stop();
            Board.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
            _mainWindow.Tiles.Clear();
        }

        private void SharedMenuCommands_OnGameStarted(object sender, EventArgs e)
        {
            remainingTime = TimeSpan.FromMinutes(App.userData.gameData.timeTotal); // Set game 
            StartGameTimer();
        }

        private void StartGameTimer()
        {
            if (gameTimer != null)
                gameTimer.Stop();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            isGameActive = true;
            Board.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Hidden;

            _mainWindow.GridRows = App.userData.gameData.sizeN;
            _mainWindow.GridColumns = App.userData.gameData.sizeM;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isGameActive)
            {
                gameTimer.Stop();
                return;
            }

            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

            TimeLabel.Text ="  " + remainingTime.ToString(@"mm\:ss");

            App.userData.gameData.timeLeft = (float)remainingTime.TotalSeconds;


            if (remainingTime <= TimeSpan.Zero)
            {
                gameTimer.Stop();
                isGameActive = false;
                App.userData.playedGames += 1;
                MessageBox.Show("Time's up!");
                Board.Visibility = Visibility.Hidden;
                Menu.Visibility = Visibility.Visible;
                _mainWindow.Tiles.Clear();
                App.userData.gameData = null;
                SaveManager.Instance.Save(App.userData.username);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Tick -= GameTimer_Tick;
                gameTimer = null;
            }

            App.SharedMenuCommands.OnGameStarted -= SharedMenuCommands_OnGameStarted;
            App.SharedMenuCommands.OnWin -= SharedMenuCommands_OnWin;
            App.SharedMenuCommands.OnGameLoaded -= SharedMenuCommands_OnGameLoaded;
        }
    }
}
