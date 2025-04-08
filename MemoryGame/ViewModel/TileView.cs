using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using MemoryGame.ViewModel.Commands;
using System.Windows.Input;


namespace MemoryGame.ViewModel
{
    public class TileView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath)); 
            }
        }
        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _isRevealed ? _image : HiddenImage;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public static BitmapImage HiddenImage = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/Images/back.png"));
        public static BitmapImage EmptyImage = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/Images/empty.png"));

        private bool _isRevealed;
        public bool IsRevealed
        {
            get => _isRevealed;
            set
            {
                _isRevealed = value;
                OnPropertyChanged(nameof(IsRevealed));
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand FlipCommand { get; set; }
        public TileView(string imagePath, MenuCommands menuCommands)
        {
            ImagePath = imagePath;
            FlipCommand = new RelayCommand(_ => menuCommands.HandleTileClick(this));
            _image = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/{ImagePath}"));
        }


        public void Flip()
        {
            if(Image == EmptyImage) 
            {
                return;
            }
            IsRevealed = !IsRevealed; 
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
