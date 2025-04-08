using MemoryGame.Memory;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace MemoryGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public static MenuCommands SharedMenuCommands = new MenuCommands();

        public static UserData userData = new UserData();

        public static List<BitmapImage> images = new List<BitmapImage>();

        //Images made by Dian Ratri taken from https://www.iconfinder.com/ as CC BY 4.0
        public static List<BitmapImage> cardImages = new List<BitmapImage>();

        App()
        {

            for (int i = 0; i < 6; i++)
            {
                BitmapImage image = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/Images/character{i}.png"));
                images.Add(image);
            }

            for(int i =0; i < 18;i++)
            {
                BitmapImage cardimage = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/Images/img{i}.png"));
                cardImages.Add(cardimage);
            }
        }

    }
}
