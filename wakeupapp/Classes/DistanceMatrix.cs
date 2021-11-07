using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Streaming.Adaptive;
using System.Diagnostics;

namespace wakeupapp
{

    class DistanceMatrix
    {
        public static Root3 distance_response;
        public static string mode;
        public static string origin = "";
        public static double originLat = 0.0;
        public static double originLong = 0.0;
        public static string destination = "";
        public static string arriveBy = "";
        public static string API_KEY = "";

        public async static Task<bool> GetDistanceMatrixFromInput()
        {

            var http = new HttpClient();
            string url = null;
            if (arriveBy != "")
            {
                var parsedDate = DateTime.Parse(arriveBy);
                long unixTime = ((DateTimeOffset)parsedDate).ToUnixTimeSeconds();
                Debug.WriteLine(unixTime);
                url = String.Format(
                "https://maps.googleapis.com/maps/api/distancematrix/json?mode={0}&arrival_time={1}&destinations={2}&origins={3}&units=metric&key={4}",
                    mode,
                    unixTime,
                    destination.Replace(" ", "%20"),
                    origin.Replace(" ", "%20"),
                    API_KEY);
            } else
            {
                url = String.Format(
                "https://maps.googleapis.com/maps/api/distancematrix/json?mode={0}&destinations={1}&origins={2}&units=metric&key={3}",
                    mode,
                    destination.Replace(" ", "%20"),
                    origin.Replace(" ", "%20"),
                    API_KEY);
            }
            
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Root3));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = JsonConvert.DeserializeObject<Root3>(result);

            distance_response = (Root3)serializer.ReadObject(ms);
            Debug.WriteLine(distance_response.status);
            Debug.WriteLine(distance_response.rows[0].elements[0].duration.text);
            return true;
        }

        public async static Task<bool> GetDistanceMatrixFromLocation()
        {

            var http = new HttpClient();
            string url = null;
            if (arriveBy != "")
            {
                var parsedDate = DateTime.Parse(arriveBy);
                long unixTime = ((DateTimeOffset)parsedDate).ToUnixTimeSeconds();
                Debug.WriteLine(unixTime);
                url = String.Format(
                    "https://maps.googleapis.com/maps/api/distancematrix/json?mode={0}&arrival_time={1}&destinations={2}&origins={3},{4}&units=metric&key={5}",
                    mode,
                    unixTime,
                    destination.Replace(" ", "%20"),
                    originLat,
                    originLong,
                    API_KEY);
            }
            else
            {
                url = String.Format(
                    "https://maps.googleapis.com/maps/api/distancematrix/json?mode={0}&destinations={1}&origins={2},{3}&units=metric&key={4}",
                    mode,
                    destination.Replace(" ", "%20"),
                    originLat,
                    originLong,
                    API_KEY);
            }
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Root3));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = JsonConvert.DeserializeObject<Root3>(result);

            distance_response = (Root3)serializer.ReadObject(ms);
            Debug.WriteLine(distance_response.status);
            Debug.WriteLine(distance_response.rows[0].elements[0].duration.text);
            return true;
        }
    }

    [DataContract]
    public class Distance
    {
        [DataMember]
        public string text { get; set; }

        [DataMember]
        public int value { get; set; }
    }

    [DataContract]
    public class Duration
    {
        [DataMember]
        public string text { get; set; }

        [DataMember]
        public int value { get; set; }
    }

    [DataContract]
    public class Elements
    {
        [DataMember]
        public Distance distance { get; set; }

        [DataMember]
        public Duration duration { get; set; }

        [DataMember]
        public string status { get; set; }

    }

    [DataContract]
    public class Rows
    {
        [DataMember]
        public List<Elements> elements { get; set; }

    }

    [DataContract]
    public class Root3
    {
        [DataMember]
        public List<string> destination_addresses { get; set; }
        [DataMember]
        public List<string> origin_addresses { get; set; }
        [DataMember]
        public List<Rows> rows { get; set; }
        [DataMember]
        public string status { get; set; }
    }
}