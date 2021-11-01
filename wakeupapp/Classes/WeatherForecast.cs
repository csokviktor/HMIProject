using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace wakeupapp.Classes
{
    class Weather_forecast
    {
        public static Root weather_forecasts;
        public static Root2 citys;
        static public double lon;
        static public double lat;

        public async static Task<bool> GetWeatherForecastInformations()
        {
            var http = new HttpClient();
            string url = String.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude=current,minutely,daily,alerts&appid=fddc3f024e2f2d470a0582adfc97d810&units=metric", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Root));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = JsonConvert.DeserializeObject<Root>(result);

            weather_forecasts = (Root)serializer.ReadObject(ms);
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }

        public async static Task<bool> ConvertCityToCoordinates(string cityName)
        {
            var http2 = new HttpClient();
            if (cityName == "")
                cityName = "Budapest";
            string url2 = String.Format("http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=1&appid=fddc3f024e2f2d470a0582adfc97d810", cityName);
            var response2 = await http2.GetAsync(url2);
            var result2 = await response2.Content.ReadAsStringAsync();
            result2 = result2.Remove(0, 1);
            result2 = result2.Remove(result2.Length - 1, 1);
            var serializer2 = new DataContractJsonSerializer(typeof(Root2));
            var ms2 = new MemoryStream(Encoding.UTF8.GetBytes(result2));
            var data2 = JsonConvert.DeserializeObject<Root2>(result2);

            citys = (Root2)serializer2.ReadObject(ms2);
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }

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
    public class Rain
    {
        [DataMember]
        public double _1h { get; set; }
    }
    [DataContract]
    public class Hourly
    {
        [DataMember]
        public int dt { get; set; }
        [DataMember]
        public double temp { get; set; }
        [DataMember]
        public double feels_like { get; set; }
        [DataMember]
        public int pressure { get; set; }
        [DataMember]
        public int humidity { get; set; }
        [DataMember]
        public double dew_point { get; set; }
        [DataMember]
        public double uvi { get; set; }
        [DataMember]
        public int clouds { get; set; }
        [DataMember]
        public int visibility { get; set; }
        [DataMember]
        public double wind_speed { get; set; }
        [DataMember]
        public int wind_deg { get; set; }
        [DataMember]
        public double wind_gust { get; set; }
        [DataMember]
        public List<Weather> weather { get; set; }
        [DataMember]
        public double pop { get; set; }
        [DataMember]
        public Rain rain { get; set; }
    }
    [DataContract]
    public class Root
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lon { get; set; }
        [DataMember]
        public string timezone { get; set; }
        [DataMember]
        public int timezone_offset { get; set; }
        [DataMember]
        public List<Hourly> hourly { get; set; }
    }
    [DataContract]
    public class LocalNames
    {
        [DataMember]
        public string af { get; set; }
        [DataMember]
        public string ar { get; set; }
        [DataMember]
        public string ascii { get; set; }
        [DataMember]
        public string az { get; set; }
        [DataMember]
        public string bg { get; set; }
        [DataMember]
        public string ca { get; set; }
        [DataMember]
        public string da { get; set; }
        [DataMember]
        public string de { get; set; }
        [DataMember]
        public string el { get; set; }
        [DataMember]
        public string en { get; set; }
        [DataMember]
        public string eu { get; set; }
        [DataMember]
        public string fa { get; set; }
        [DataMember]
        public string feature_name { get; set; }
        [DataMember]
        public string fi { get; set; }
        [DataMember]
        public string fr { get; set; }
        [DataMember]
        public string gl { get; set; }
        [DataMember]
        public string he { get; set; }
        [DataMember]
        public string hi { get; set; }
        [DataMember]
        public string hr { get; set; }
        [DataMember]
        public string hu { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string it { get; set; }
        [DataMember]
        public string ja { get; set; }
        [DataMember]
        public string la { get; set; }
        [DataMember]
        public string lt { get; set; }
        [DataMember]
        public string mk { get; set; }
        [DataMember]
        public string nl { get; set; }
        [DataMember]
        public string no { get; set; }
        [DataMember]
        public string pl { get; set; }
        [DataMember]
        public string pt { get; set; }
        [DataMember]
        public string ro { get; set; }
        [DataMember]
        public string ru { get; set; }
        [DataMember]
        public string sk { get; set; }
        [DataMember]
        public string sl { get; set; }
        [DataMember]
        public string sr { get; set; }
        [DataMember]
        public string th { get; set; }
        [DataMember]
        public string tr { get; set; }
        [DataMember]
        public string vi { get; set; }
        [DataMember]
        public string zu { get; set; }
    }
    [DataContract]
    public class Root2
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public LocalNames local_names { get; set; }
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lon { get; set; }
        [DataMember]
        public string country { get; set; }
    }
}
