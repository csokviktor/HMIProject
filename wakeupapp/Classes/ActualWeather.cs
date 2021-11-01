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

namespace wakeupapp
{
    class ActualWeather
    {
        public static Root weather_reports;
        public static string search_by = "Budapest";
        public static double latitude = 0.0;
        public static double longitude = 0.0;

        public async static Task<bool> GetWeatherInformationsByName()
        {
            var http = new HttpClient();
            if (search_by == "")
                search_by = "Budapest";
            string url = String.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid=fddc3f024e2f2d470a0582adfc97d810&units=metric", search_by);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Root));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = JsonConvert.DeserializeObject<Root>(result);

            weather_reports = (Root)serializer.ReadObject(ms);
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }

        public async static Task<bool> GetWeatherInformationsByCoord()
        {
            var http = new HttpClient();
            if (latitude == 0.0 && longitude == 0.0)
            {
                latitude = 47.4979;
                longitude = 19.0402;
            }
                
            string url = String.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid=fddc3f024e2f2d470a0582adfc97d810&units=metric", latitude, longitude);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Root));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = JsonConvert.DeserializeObject<Root>(result);

            weather_reports = (Root)serializer.ReadObject(ms);
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [DataContract]
    public class Coord
    {
        [DataMember]
        public double lon { get; set; }
        [DataMember]
        public double lat { get; set; }
    }
    [DataContract]
    public class Weather
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string main { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string icon { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember]
        public double temp { get; set; }
        [DataMember]
        public double feels_like { get; set; }
        [DataMember]
        public double temp_min { get; set; }
        [DataMember]
        public double temp_max { get; set; }
        [DataMember]
        public int pressure { get; set; }
        [DataMember]
        public int humidity { get; set; }
        [DataMember]
        public int sea_level { get; set; }
        [DataMember]
        public int grnd_level { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public double speed { get; set; }
        [DataMember]
        public int deg { get; set; }
        [DataMember]
        public double gust { get; set; }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public int sunrise { get; set; }
        [DataMember]
        public int sunset { get; set; }
    }

    [DataContract]
    public class Root
    {
        [DataMember]
        public Coord coord { get; set; }
        [DataMember]
        public List<Weather> weather { get; set; }
        [DataMember]
        public string @base { get; set; }
        [DataMember]
        public Main main { get; set; }
        [DataMember]
        public int visibility { get; set; }
        [DataMember]
        public Wind wind { get; set; }
        [DataMember]
        public Clouds clouds { get; set; }
        [DataMember]
        public int dt { get; set; }
        [DataMember]
        public Sys sys { get; set; }
        [DataMember]
        public int timezone { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int cod { get; set; }
    }


}
