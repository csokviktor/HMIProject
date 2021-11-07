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
using System.Diagnostics;
using wakeupapp.Classes;
using Windows.Devices.Geolocation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsPage : Page
    {

        private ToggleButton active_button = null;

        public MapsPage()
        {
            this.InitializeComponent();
            icon_button_clicked(this.bike_button, new RoutedEventArgs());
        }

        private void arrive_by_button_clicked(object sender, RoutedEventArgs e)
        {
            active_button = (ToggleButton)sender;
            if (active_button.IsChecked == true)
            {
                Calendar.LoadJson();
                DistanceMatrix.arriveBy = Calendar.items.calendaritems[0].dateTo;
            } else
            {
                DistanceMatrix.arriveBy = "";
            }
            Debug.WriteLine(DistanceMatrix.arriveBy);
        }

        private void icon_button_clicked(object sender, RoutedEventArgs e)
        {
            active_button = (ToggleButton)sender;
            active_button.IsChecked = true;
            if (((ToggleButton)sender).Name == "bike_button")
            {
                this.car_button.IsChecked = false;
                this.public_button.IsChecked = false;
                this.walk_button.IsChecked = false;
                DistanceMatrix.mode = "bicycle";
            }
            else if (((ToggleButton)sender).Name == "car_button") {
                this.bike_button.IsChecked = false;
                this.public_button.IsChecked = false;
                this.walk_button.IsChecked = false;
                DistanceMatrix.mode = "driving";
            }
            else if (((ToggleButton)sender).Name == "public_button")
            {
                this.bike_button.IsChecked = false;
                this.car_button.IsChecked = false;
                this.walk_button.IsChecked = false;
                DistanceMatrix.mode = "transit";
            }
            else if (((ToggleButton)sender).Name == "walk_button")
            {
                this.bike_button.IsChecked = false;
                this.car_button.IsChecked = false;
                this.public_button.IsChecked = false;
                DistanceMatrix.mode = "walking";
            }
        }
        
        private async void search_button_clicked(object sender, RoutedEventArgs e)
        {
            if (this.maps_ending_input.Text == "" || this.maps_starting_input.Text == "")
            {
                handle_error_msg("Please specify an Origin and a Destination");
            }
            else
            {
                this.estimated_times.Text = "Calculating routes";
                DistanceMatrix.origin = this.maps_starting_input.Text;
                DistanceMatrix.destination = this.maps_ending_input.Text;
                var flag = await DistanceMatrix.GetDistanceMatrixFromInput();

                TimeSpan t = TimeSpan.FromSeconds(DistanceMatrix.distance_response.rows[0].elements[0].duration.value);
                string travel_time = String.Format(
                    "{0:D2}:{1:D2}:{2:D2}",
                    t.Hours,
                    t.Minutes,
                    t.Seconds
                    );
                DateTime current_time = DateTime.Now;
                DateTime arrival_time = current_time.AddSeconds(DistanceMatrix.distance_response.rows[0].elements[0].duration.value);

                if (DistanceMatrix.arriveBy == "")
                {
                    this.estimated_times.Text = String.Format(
                        "Your estimated travel time is {0} so you will arrive at {1} to {2}",
                        travel_time,
                        arrival_time.ToString("HH:mm:ss"),
                        DistanceMatrix.distance_response.destination_addresses[0]
                        );
                }
                else
                {
                    var parsedDate = DateTime.Parse(DistanceMatrix.arriveBy);
                    var leaveDate = parsedDate.Subtract(t);
                    var leaveDateString = leaveDate.ToString("HH:mm:ss");
                    this.estimated_times.Text = String.Format(
                        "Your estimated travel time is {0} so you have to leave at {1} to arrive at {2} to {3}",
                        travel_time,
                        leaveDateString,
                        parsedDate.ToString("HH:mm:ss"),
                        DistanceMatrix.distance_response.destination_addresses[0]
                        );
                }
            }
        }

        private async void handle_error_msg(string msg)
        {
            this.error_msg.Visibility = Visibility.Visible;
            this.error_msg.Text = msg;
            await System.Threading.Tasks.Task.Delay(1500);
            this.error_msg.Visibility = Visibility.Collapsed;
            this.error_msg.Text = "";
        }

        private async void search_current_location_button_clicked(object sender, RoutedEventArgs e)
        {
            if (this.maps_ending_input.Text == "")
            {
                handle_error_msg("Please specify a Destination");
            }
            else
            {
                this.estimated_times.Text = "Calculating routes";
                Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
                Geoposition pos = await geolocator.GetGeopositionAsync();

                DistanceMatrix.originLat = pos.Coordinate.Latitude;
                DistanceMatrix.originLong = pos.Coordinate.Longitude;

                DistanceMatrix.destination = this.maps_ending_input.Text;
                var flag = await DistanceMatrix.GetDistanceMatrixFromLocation();

                TimeSpan t = TimeSpan.FromSeconds(DistanceMatrix.distance_response.rows[0].elements[0].duration.value);
                string travel_time = String.Format(
                    "{0:D2}:{1:D2}:{2:D2}",
                    t.Hours,
                    t.Minutes,
                    t.Seconds
                    );
                DateTime current_time = DateTime.Now;
                DateTime arrival_time = current_time.AddSeconds(DistanceMatrix.distance_response.rows[0].elements[0].duration.value);

                if (DistanceMatrix.arriveBy == "")
                {
                    this.estimated_times.Text = String.Format(
                        "Your estimated travel time is {0} so you will arrive at {1} to {2}",
                        travel_time,
                        arrival_time.ToString("HH:mm:ss"),
                        DistanceMatrix.distance_response.destination_addresses[0]
                        );
                }
                else
                {
                    var parsedDate = DateTime.Parse(DistanceMatrix.arriveBy);
                    var leaveDate = parsedDate.Subtract(t);
                    var leaveDateString = leaveDate.ToString("HH:mm:ss");
                    this.estimated_times.Text = String.Format(
                        "Your estimated travel time is {0} so you have to leave at {1} to arrive at {2} to {3}",
                        travel_time,
                        leaveDateString,
                        parsedDate.ToString("HH:mm:ss"),
                        DistanceMatrix.distance_response.destination_addresses[0]
                        );
                }
            }
        }
    }
}
