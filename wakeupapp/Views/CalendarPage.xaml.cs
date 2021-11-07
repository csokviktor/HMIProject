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
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalendarPage : Page
    {

        public CalendarPage()
        {
            this.InitializeComponent();
            Calendar.LoadJson();
            DrawerListOptions.ItemsSource = Calendar.items.calendaritems;
            ObservableCollection<TitleBar> tb = new ObservableCollection<TitleBar>();
            tb.Add(new TitleBar("Title", "From", "To", "Location"));
            TitleList.ItemsSource = tb;
        }

        private void MySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView li = sender as ListView;
        }

        public class TitleBar
        {
            public String Title { get; set; }
            public String From { get; set; }
            public String To { get; set; }
            public String Location { get; set; }

            public TitleBar(String title, String from, String to, String location)
            {
                this.Title = title;
                this.From = from;
                this.To = to;
                this.Location = location;
            }

        }
    }
}
