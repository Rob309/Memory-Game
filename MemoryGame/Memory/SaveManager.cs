using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemoryGame.Model;
using Newtonsoft.Json;

namespace MemoryGame.Memory
{
    public class SaveManager
    {
        // lazy-loading
        private static readonly Lazy<SaveManager> _instance = new Lazy<SaveManager>(() => new SaveManager());
        public static SaveManager Instance => _instance.Value;
       
        private string saveRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves");

        private SaveManager()
        {
            
        }

        public void Save(string username)
        {
            try
            {
                if (!Directory.Exists(saveRootPath))
                    Directory.CreateDirectory(saveRootPath);

                string userDirectory = Path.Combine(saveRootPath, username);
                if (!Directory.Exists(userDirectory))
                    Directory.CreateDirectory(userDirectory);

                string jsonFilePath = Path.Combine(userDirectory, $"{username}.json");

                string jsonData = JsonConvert.SerializeObject(App.userData, Formatting.Indented);

                File.WriteAllText(jsonFilePath, jsonData);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Error: Access denied when saving data. {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error: There was an issue with the file system when saving data. {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error while saving data: {ex.Message}");
            }
        }

        public void DeleteUserData(string username)
        {
            try
            {
                if (!Directory.Exists(saveRootPath))
                    Directory.CreateDirectory(saveRootPath);

                string userDirectory = Path.Combine(saveRootPath, username);
                
                if (Directory.Exists(userDirectory)) 
                {
                    Directory.Delete(userDirectory, true); 
                }
                else
                {
                    MessageBox.Show($"Error: The user directory '{username}' does not exist.");
                }

            }
            catch (UnauthorizedAccessException ex)
            {

                MessageBox.Show($"Error: Access denied when loading data. {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error while loading data: {ex.Message}");
            }
        }
        public void Load(string username)
        {
            try
            {
                if (!Directory.Exists(saveRootPath))
                    Directory.CreateDirectory(saveRootPath);

                string userDirectory = Path.Combine(saveRootPath, username);
                string jsonFilePath = Path.Combine(userDirectory, $"{username}.json");

                if (File.Exists(jsonFilePath))
                {
                    string jsonData = File.ReadAllText(jsonFilePath);
                    App.userData = JsonConvert.DeserializeObject<UserData>(jsonData);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                
                MessageBox.Show($"Error: Access denied when loading data. {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error: There was an issue with the file system when loading data. {ex.Message}");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error: Failed to deserialize the data. {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error while loading data: {ex.Message}");
            }

        }

        public List<string> LoadFolders()
        {
            List<string> saves = new List<string>();


            if (!Directory.Exists(saveRootPath))
                Directory.CreateDirectory(saveRootPath);

            var directories = Directory.GetDirectories(saveRootPath);

            foreach (var dir in directories)
            {
                saves.Add(Path.GetFileName(dir)); 
            }

            return saves;
        }
    }
}
