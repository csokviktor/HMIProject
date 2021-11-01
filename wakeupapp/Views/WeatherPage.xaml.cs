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
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherPage : Page
    {
        public WeatherPage()
        {
            this.InitializeComponent();

            weather_button_Click(this, new RoutedEventArgs());
        }

        private async void weather_button_Click(object sender, RoutedEventArgs e)
        {
            var flag = await ActualWeather.GetWeatherInformationsByName();
            var flag3 = await Weather_forecast.ConvertCityToCoordinates(weather_search_input_tb.Text);
            Weather_forecast.lat = Weather_forecast.citys.lat;
            Weather_forecast.lon = Weather_forecast.citys.lon;
            var flag2 = await Weather_forecast.GetWeatherForecastInformations();
            try
            {
                string weather_icon = String.Format("http://openweathermap.org/img/wn/{0}@2x.png", ActualWeather.weather_reports.weather[0].icon);
                Weather_img.Source = new BitmapImage(new Uri(weather_icon, UriKind.Absolute));
                ///forecast_slider.Value = 1;
                fill_weather_textblock();
                fill_forecast_textblock();
            }
            catch (Exception ex)
            {

            }
        }

        private void weather_search_input_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualWeather.search_by = weather_search_input_tb.Text;
        }

        private void fill_weather_textblock()
        {
            weather_textblock_name.Text = ActualWeather.weather_reports.name;
            weather_textblock_temp.Text = ActualWeather.weather_reports.main.temp + "°C";
            weather_textblock_image.Text = ActualWeather.weather_reports.weather[0].description;
            weather_textblock.Text = "Wind: " + Math.Round((3.6 * ActualWeather.weather_reports.wind.speed), 2) + " km/h" + "\nHumidity: " + ActualWeather.weather_reports.main.humidity + " %" + "\nPressure: " + ActualWeather.weather_reports.main.pressure + " Pa";
        }
        private void fill_forecast_textblock()
        {
            DateTime currentTime = DateTime.Now;
            var cntr = 0;
            for (int i = 0; i < 12; i++)
            {
                cntr += 2;
                string weather_icon = String.Format("http://openweathermap.org/img/wn/{0}@2x.png", Weather_forecast.weather_forecasts.hourly[cntr].weather[0].icon);
                TextBlock txt = this.FindName(String.Format("forecast_textblock_temp{0}", i)) as TextBlock;
                txt.Text = Weather_forecast.weather_forecasts.hourly[cntr].temp.ToString() + "°C";
                Image img = this.FindName(String.Format("forecast_image{0}", i)) as Image;
                img.Source = new BitmapImage(new Uri(weather_icon, UriKind.Absolute));
                TextBlock timeTag = this.FindName(String.Format("forecast_textblock_time{0}", i)) as TextBlock;
                DateTime tempTime = currentTime.AddHours((double)cntr);
                timeTag.Text = String.Format("Prediction for {0}", tempTime.ToString("HH:mm"));
            }
        }

    }
}
