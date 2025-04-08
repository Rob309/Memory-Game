using MemoryGame.Memory;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using MemoryGame.View;
using MemoryGame.Model;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;


namespace MemoryGame.ViewModel.Commands
{
    public class MenuCommands
    {
        #region CreateAccount

        #region SaveNewUser

        private ICommand _SaveNewAccountCommand;
        public ICommand SaveNewAccountCommand
        {
            get
            {
                if (_SaveNewAccountCommand == null)
                    _SaveNewAccountCommand = new RelayCommand(SaveNewAccount);
                return _SaveNewAccountCommand;
            }
        }

        void SaveNewAccount(object parameter)
        {
            TextBox textBox = parameter as TextBox;
            if (textBox != null)
            {
                App.userData.username = textBox.Text;
                if (!IsValidFolderName(App.userData.username))
                {
                    MessageBox.Show("Invalid username");
                    return;
                }
                SaveManager.Instance.Save(App.userData.username);

                List<string> saveList = SaveManager.Instance.LoadFolders();

                foreach (var user in saveList)
                {
                    if (!Users.Contains(user))
                    {
                        Users.Add(user);
                    }
                }

                Window parentWindow = Window.GetWindow(textBox);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
        }

        public bool IsValidFolderName(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();

            if (folderName.Any(c => invalidChars.Contains(c)))
            {
                return false;
            }

            if (folderName.Length > 255)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region CancelCreateAccount

        private ICommand _CancelCreateAccountCommand;
        public ICommand CancelCreateAccountCommand
        {
            get
            {
                if (_CancelCreateAccountCommand == null)
                    _CancelCreateAccountCommand = new RelayCommand(CancelCreateAccount);
                return _CancelCreateAccountCommand;
            }
        }

        void CancelCreateAccount(object parameter)
        {
            Window createAccountWindow = parameter as Window;
            if (createAccountWindow != null)
            {
                createAccountWindow.Close();
            }
        }

        #endregion

        #region ImageLeft

        private ICommand _ImageLeftCommand;
        public ICommand ImageLeftCommand
        {
            get
            {
                if (_ImageLeftCommand == null)
                    _ImageLeftCommand = new RelayCommand(ImageLeft);
                return _ImageLeftCommand;
            }
        }

        void ImageLeft(object parameter)
        {
            if (App.userData.imageID == 0)
            {
                App.userData.imageID = App.images.Count - 1;
            }
            else
            {
                App.userData.imageID--;
            }

            Frame imageFrame = parameter as Frame;

            if (imageFrame != null)
            {
                ImageBrush brush = new ImageBrush(App.images[App.userData.imageID]);
                imageFrame.Background = brush;
            }
        }

        #endregion

        #region ImageRight

        private ICommand _ImageRightCommand;
        public ICommand ImageRightCommand
        {
            get
            {
                if (_ImageRightCommand == null)
                    _ImageRightCommand = new RelayCommand(ImageRight);
                return _ImageRightCommand;
            }
        }

        void ImageRight(object parameter)
        {
            if (App.userData.imageID == 5)
            {
                App.userData.imageID = 0;
            }
            else
            {
                App.userData.imageID++;
            }

            Frame imageFrame = parameter as Frame;

            if (imageFrame != null)
            {
                ImageBrush brush = new ImageBrush(App.images[App.userData.imageID]);
                imageFrame.Background = brush;
            }
        }

        #endregion

        #endregion

        #region Login

        #region Delete User

        public ObservableCollection<string> Users { get; set; }

        public void SetUsers(ObservableCollection<string> users)
        {
            Users = users;
        }

        private ICommand _DeleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (_DeleteUserCommand == null)
                    _DeleteUserCommand = new RelayCommand(DeleteUser);
                return _DeleteUserCommand;
            }
        }

        void DeleteUser(object parameter)
        {
            ListBox memoryList = parameter as ListBox;
            if (memoryList != null)
            {
                if (App.userData.username == null)
                {
                    return;
                }
                var selectedItem = memoryList.SelectedItem as string;
                if (selectedItem != null)
                {

                    Users.Remove(selectedItem);

                    SaveManager.Instance.DeleteUserData(App.userData.username);
                }
            }
        }

        #endregion

        #region Create User

        private ICommand _CreateUserCommand;
        public ICommand CreateUserCommand
        {
            get
            {
                if (_CreateUserCommand == null)
                    _CreateUserCommand = new RelayCommand(CreateUser);
                return _CreateUserCommand;
            }
        }

        void CreateUser(object parameter)
        {
            var createAccountWindow = new CreateAccount();

            //show dialog blocks login clicks until closed
            createAccountWindow.ShowDialog();
        }

        #endregion

        #region Select User

        private ICommand _SelectedUserCommand;
        public ICommand SelectedUserCommand
        {
            get
            {
                if (_SelectedUserCommand == null)
                    _SelectedUserCommand = new RelayCommand(SelectedUser);
                return _SelectedUserCommand;
            }
        }

        public event EventHandler OnChangeSelected;
        void SelectedUser(object parameter)
        {

            var selectedItem = parameter as string;
            if (selectedItem != null)
            {

                SaveManager.Instance.Load(selectedItem);

                OnChangeSelected?.Invoke(this, new EventArgs());
            }

        }

        #endregion

        #region Play

        private ICommand _PlayCommand;
        public ICommand PlayCommand
        {
            get
            {
                if (_PlayCommand == null)
                    _PlayCommand = new RelayCommand(Play);
                return _PlayCommand;
            }
        }
        void Play(object parameter)
        {

            ListBox memoryList = parameter as ListBox;
            if (memoryList != null)
            {
                if (memoryList.SelectedItem != null)
                {
                    var mainWindow = new MainWindow();

                    //show dialog blocks login clicks until closed
                    mainWindow.Show();

                    Window parentWindow = Window.GetWindow(memoryList);
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                }
                else { MessageBox.Show("Please select a player"); }
            }

        }

        #endregion

        #endregion

        #region GameWindow

        #region Exit

        private ICommand _ExitGameCommand;
        public ICommand ExitGameCommand
        {
            get
            {
                if (_ExitGameCommand == null)
                    _ExitGameCommand = new RelayCommand(ExitGame);
                return _ExitGameCommand;
            }
        }

        void ExitGame(object parameter)
        {
            MainWindow gameWindow = parameter as MainWindow;
            if (gameWindow != null)
            {
                LogIn loginMenu = new LogIn();

                loginMenu.Show();

                gameWindow.isGameActive = false;

                gameWindow.Close();
            }
        }

        #endregion

        #region Statistics

        private ICommand _StatisticsCommand;
        public ICommand StatisticsCommand
        {
            get
            {
                if (_StatisticsCommand == null)
                    _StatisticsCommand = new RelayCommand(Stats);
                return _StatisticsCommand;
            }
        }

        void Stats(object parameter)
        {
            Statistics stats = new Statistics(App.userData.wonGames, App.userData.playedGames);

            stats.Show();

        }

        #endregion

        #region Open Game

        private ICommand _OpenGameCommand;
        public ICommand OpenGameCommand
        {
            get
            {
                if (_OpenGameCommand == null)
                    _OpenGameCommand = new RelayCommand(OpenGame);
                return _OpenGameCommand;
            }
        }

        void OpenGame(object parameter)
        {
            if (App.userData.gameData != null)
            {
                LoadGame();
            }
            else
            {
                MessageBox.Show("No game saved");
            }

        }

        public event EventHandler OnGameLoaded;
        void LoadGame()
        {
            gameData = App.userData.gameData;

            CreateTiles();

            OnGameLoaded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region New Game

        private ICommand _NewGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (_NewGameCommand == null)
                    _NewGameCommand = new RelayCommand(NewGame);
                return _NewGameCommand;
            }
        }

        void NewGame(object parameter)
        {
            GameSettings settings = new GameSettings();

            settings.ShowDialog();

        }

        #endregion

        #region Save Game

        private ICommand _SaveGameCommand;
        public ICommand SaveGameCommand
        {
            get
            {
                if (_SaveGameCommand == null)
                    _SaveGameCommand = new RelayCommand(SaveGame);
                return _SaveGameCommand;
            }
        }

        void SaveGame(object parameter)
        {
            CreateSaveString();
            SaveManager.Instance.Save(App.userData.username);
        }

        #endregion

        #endregion

        #region GameSettings

        #region Standard

        private ICommand _StandardCommand;
        public ICommand StandardCommand
        {
            get
            {
                if (_StandardCommand == null)
                    _StandardCommand = new RelayCommand(Standard);
                return _StandardCommand;
            }
        }

        public ObservableCollection<TileView> GameTiles { get; set; }

        GameData gameData;
        public event EventHandler OnGameStarted;
        void Standard(object parameter)
        {
            Window window = parameter as Window;

            gameData = new GameData();

            gameData.timeTotal = 2;
            gameData.sizeM = 4;
            gameData.sizeN = 4;

            gameData.boardData = GenerateTileString();
            App.userData.gameData = gameData;

            CreateTiles();

            OnGameStarted?.Invoke(this, EventArgs.Empty);

            if (window != null)
            {
                window.Close();
            }
        }


        #endregion

        #region Custom Game

        private ICommand _CustomCommand;
        public ICommand CustomCommand
        {
            get
            {
                if (_CustomCommand == null)
                    _CustomCommand = new RelayCommand(Custom);
                return _CustomCommand;
            }
        }

        void Custom(object parameter)
        {
            Window gameSettings = parameter as Window;
            
            CustomSettings customSettings = new CustomSettings();

            customSettings.Show();

            if (gameSettings != null)
            {
                gameSettings.Close();
            }

        }

        #endregion

        #region Back

        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                if (_BackCommand == null)
                    _BackCommand = new RelayCommand(Back);
                return _BackCommand;
            }
        }

        void Back(object parameter)
        {
            Window customSettings = parameter as Window;

            GameSettings gameSettings = new GameSettings();

            gameSettings.Show();

            if (customSettings != null)
            {
                customSettings.Close();
            }

        }

        #endregion

        #region Start

        private ICommand _StartCustomCommand;
        public ICommand StartCustomCommand
        {
            get
            {
                if (_StartCustomCommand == null)
                    _StartCustomCommand = new RelayCommand(StartCustom);
                return _StartCustomCommand;
            }
        }

        void StartCustom(object parameter)
        {
            if (parameter is Window window)
            {
                var timeBox = window.FindName("TimeBox") as TextBox;
                var widthBox = window.FindName("WidthBox") as TextBox;
                var heightBox = window.FindName("HeightBox") as TextBox;

                if (timeBox != null && widthBox != null && heightBox != null)
                {
                    string timeText = timeBox.Text;
                    string widthText = widthBox.Text;
                    string heightText = heightBox.Text;

                    if (float.TryParse(timeText, out float time) &&
                        int.TryParse(widthText, out int width) &&
                        int.TryParse(heightText, out int height))
                    {
                        if (time <= 0 || time >10) 
                        {
                            MessageBox.Show("Invalid Time, use values from 0 to 10");
                            return;
                        }
                        if(width<1 || width > 6)
                        {
                            MessageBox.Show("Invalid With, use values from 1 to 6");
                            return;
                        }
                        if (height < 1 || height > 6)
                        {
                            MessageBox.Show("Invalid Height, use values from 1 to 6");
                            return;
                        }

                        if(width * height % 2 == 1)
                        {
                            MessageBox.Show("Invalid Size, total size should be divisible with 2. Your size: " + width*height);
                            return;
                        }

                        gameData = new GameData();

                        gameData.timeTotal = time;
                        gameData.sizeM = width;
                        gameData.sizeN = height;

                        gameData.boardData = GenerateTileString();
                        App.userData.gameData = gameData;

                        CreateTiles();

                        OnGameStarted?.Invoke(this, EventArgs.Empty);

                    }
                    else
                    {
                        MessageBox.Show("Invalid inputs, should be using numbers");
                        return;
                    }
                }

                window.Close();
            }

        }

        #endregion

        #endregion

        #region Utility


        string GenerateTileString()
        {
            string tileString = "";
            List<int> tileId = new List<int>();
            List<int> tileValue = Enumerable.Repeat(0, gameData.sizeN * gameData.sizeM).ToList();
            for (int i = 0; i < gameData.sizeN * gameData.sizeM; i++)
            {
                tileId.Add(i);
            }

            Random random = new Random();
            int pictureId = 0;
            while (tileId.Count >= 2)
            {
                int index1 = random.Next(tileId.Count);
                int index2;
                // Ensure the second index is different from the first
                do
                {
                    index2 = random.Next(tileId.Count);
                } while (index1 == index2);

                tileValue[tileId[index1]] = pictureId;
                tileValue[tileId[index2]] = pictureId++;

                if (index1 > index2)
                {
                    tileId.RemoveAt(index1);
                    tileId.RemoveAt(index2);
                }
                else
                {
                    tileId.RemoveAt(index2);
                    tileId.RemoveAt(index1);
                }
            }
            for (int i = 0; i < tileValue.Count; i++)
            {
                tileString += tileValue[i].ToString();
                tileString += ".";
            }

            return tileString;
        }

        void CreateTiles()
        {
            string[] values = gameData.boardData.Split('.');

            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if(int.Parse(value) == -1)
                    {
                        string imagePath = "Images/back.png";
                        TileView tile = new TileView(imagePath, App.SharedMenuCommands);
                        tile.Image = TileView.EmptyImage;
                        tile.Flip();
                        tileRemovedCount++;
                        GameTiles.Add(tile);
                    }
                    else
                    {
                        string imagePath = "Images/img" + value + ".png";
                        TileView tile = new TileView(imagePath, App.SharedMenuCommands);
                        GameTiles.Add(tile);
                    }
                }
            }
        }

        void CreateSaveString()
        {
            App.userData.gameData.boardData = "";
            foreach (var tile in GameTiles)
            {
                if (tile.Image != TileView.EmptyImage) 
                {
                    Match match = Regex.Match(tile.ImagePath, @"\d+");

                    if (match.Success)
                    {
                        App.userData.gameData.boardData += match.Value +"."; // number will be 15
                    }
                }
                else 
                { 
                    App.userData.gameData.boardData += "-1.";
                }
            }
        }

        TileView tile1, tile2;

        int tileRemovedCount = 0;

        public event EventHandler OnWin;
        public async void HandleTileClick(TileView tile)
        {
            if(tile1 == null)
            {
                tile1 = tile;
                tile1.Flip();
            }
            else 
            {
                if(tile2 == null && tile != tile1)
                {
                    tile2 = tile;
                    tile2.Flip();

                    if (tile1.ImagePath == tile2.ImagePath)
                    {
                        await Task.Delay(1000);

                        tile1.Image = TileView.EmptyImage;
                        tile2.Image = TileView.EmptyImage;
                        tileRemovedCount += 2;

                        tile1 = null;
                        tile2 = null;

                        if(tileRemovedCount == GameTiles.Count)
                        {
                            App.userData.playedGames += 1;
                            App.userData.wonGames += 1;
                            MessageBox.Show("You won!");
                            App.userData.gameData = null;
                            SaveManager.Instance.Save(App.userData.username);
                            tileRemovedCount = 0;
                            OnWin?.Invoke(this, new EventArgs());
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
                        if (tile1 != null)
                        {
                            tile1.Flip();
                        }
                        if(tile2 != null)
                        {
                            tile2.Flip();
                        }

                        tile1 = null;
                        tile2 = null;
                    }
                }

                if(tile == tile1)
                {
                    tile1.Flip();
                    tile1 = null;
                }
            }


        }

        #endregion
    }
}
