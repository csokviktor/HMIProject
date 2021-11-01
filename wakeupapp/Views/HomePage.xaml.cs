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
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();

        public HomePage()
        {
            this.InitializeComponent();

            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            weather_button_Click(this, new RoutedEventArgs());
        }

        private void Timer_Tick(object sender, object e)
        {
            Time.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private async void weather_button_Click(object sender, RoutedEventArgs e)
        {
            weather_textblock_temp.Text = "Loading weather.";
            try {
                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                Geoposition pos = await geolocator.GetGeopositionAsync();
                ActualWeather.latitude = pos.Coordinate.Latitude;
                ActualWeather.longitude = pos.Coordinate.Longitude;
            } catch(Exception er) {
                Debug.WriteLine(er);
            }
            
            var flag = await ActualWeather.GetWeatherInformationsByCoord();
            try
            {
                string weather_icon = String.Format("http://openweathermap.org/img/wn/{0}@2x.png", ActualWeather.weather_reports.weather[0].icon);
                Weather_img.Source = new BitmapImage(new Uri(weather_icon, UriKind.Absolute));
                fill_weather_textblock();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void fill_weather_textblock()
        {
            weather_textblock_temp.Text = ActualWeather.weather_reports.main.temp + "°C";
            weather_textblock_image.Text = ActualWeather.weather_reports.weather[0].description;
        }
    }
}
