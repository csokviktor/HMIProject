using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using wakeupapp.Classes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public int selectedIndex;
        public TransformedUserSearch selectedItem = new TransformedUserSearch();
        public ObservableCollection<TransformedUserSearch> userSearch;
        public SettingsPage()
        {
            this.InitializeComponent();
            News.LoadUserJson();
            user_search_transformer();
            this.TitleList.ItemsSource = userSearch;

        }

        private void user_search_transformer()
        {
            userSearch = new ObservableCollection<TransformedUserSearch>();
            foreach (string title in News.user_search_by.searchElements)
            {
                Debug.WriteLine(title);
                userSearch.Add(new TransformedUserSearch() { searchElement = title });
            }
        }


        private void ItemGotSelected_Filter(object sender, SelectionChangedEventArgs e)
        {
        }

        public class TransformedUserSearch
        {
            public string searchElement { get; set; }
        }

        private void delete_topic_button_Click(object sender, RoutedEventArgs e)
        {
            userSearch.Remove(selectedItem);
            UpdateJSON();
        }

        private void add_topic_button_Click(object sender, RoutedEventArgs e)
        {
            if(topic_input.Text == "")
            {
                handle_error_msg("Please specify a new Topic");
                return;
            }
            userSearch.Add(new TransformedUserSearch() { searchElement = topic_input.Text });
            UpdateJSON();
        }

        private async void handle_error_msg(string msg)
        {
            this.error_msg.Visibility = Visibility.Visible;
            this.error_msg.Text = msg;
            await System.Threading.Tasks.Task.Delay(1500);
            this.error_msg.Visibility = Visibility.Collapsed;
            this.error_msg.Text = "";
        }

        public void UpdateJSON()
        {
            News.UserSearchBy writeToJSON = new News.UserSearchBy();
            writeToJSON.searchElements = new List<string>();
            foreach (TransformedUserSearch temp in userSearch)
            {
                writeToJSON.searchElements.Add(temp.searchElement);
            }

            string FilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "search_by.json");
            Debug.WriteLine(FilePath);
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(writeToJSON));
        }
    }
}
