using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace wakeupapp.Classes
{
    class Calendar
    {
        public static Items items;
        public static void LoadJson()
        {
            string FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "calendar.json");
            using (StreamReader file = File.OpenText(FilePath))
            {
                var json = file.ReadToEnd();
                var serializer = new DataContractJsonSerializer(typeof(Items));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var data = JsonConvert.DeserializeObject<Root>(json);

                items = (Items)serializer.ReadObject(ms);
                items.calendaritems.Sort((x, y) => DateTime.Parse(x.dateFrom).CompareTo(DateTime.Parse(y.dateFrom)));
            }
        }
    }

    

    [DataContract]
    public class CalendarItem
    {
        [DataMember]
        public string dateFrom { get; set; }

        [DataMember]
        public string dateTo { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string location { get; set; }
    }

    [DataContract]
    public class Items
    {
        [DataMember]
        public List<CalendarItem> calendaritems { get; set; }
    }
}
