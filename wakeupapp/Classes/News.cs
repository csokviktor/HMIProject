using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace wakeupapp.Classes
{
    class News
    {
        static public List<Article> news_articles; //Todo: getter settert írni
        static public List<Article> news_articles_custom;
        static public int current_news_index = 0; //Todo: getter settert írni
        static public string search_by = "Apple"; //Todo: getter settert írni
        static public UserSearchBy user_search_by;
        public async static Task<bool> GetArticlesMain()
        {
            var http = new HttpClient();
            if (search_by == "")
                search_by = "Apple";
            string url = String.Format("https://newsapi.org/v2/everything?q={0}&from=2021-9-31&sortBy=relevancy&apiKey=67d587b29a9041f5908014ecf27e3fa0", search_by);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NewsResponse_root>(result);

            news_articles = data.articles;
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }

        public async static Task<bool> GetArticlesCustom()
        {
            LoadUserJson();
            var http = new HttpClient();
            if (user_search_by.searchElements.Count() == 0)
                return false;
            string queryString = String.Join("%20OR%20", user_search_by.searchElements);
            string url = String.Format("https://newsapi.org/v2/everything?q={0}&from=2021-9-31&sortBy=relevancy&apiKey=67d587b29a9041f5908014ecf27e3fa0", queryString);
            Debug.WriteLine(url);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NewsResponse_root>(result);

            news_articles_custom = data.articles;
            return true; //Todo: hibavizsgálat visszatérésnél, nem csak simán true-t visszaadni
        }

        public static void LoadUserJson()
        {
            string appDataFile = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "search_by.json");
            string FilePath = "";
            if (File.Exists(appDataFile))
            {
                FilePath = appDataFile;
            }
            else
            {
                FilePath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "search_by.json");
            }
            using (StreamReader file = File.OpenText(FilePath))
            {
                var json = file.ReadToEnd();
                Debug.WriteLine(appDataFile);
                if (!File.Exists(appDataFile))
                {
                    File.WriteAllText(appDataFile, json);
                }
                var serializer = new DataContractJsonSerializer(typeof(UserSearchBy));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var data = JsonConvert.DeserializeObject<UserSearchBy>(json);

                user_search_by = (UserSearchBy)serializer.ReadObject(ms);
            }
        }

        [DataContract]
        public class UserSearchBy
        {
            [DataMember]
            public List<string> searchElements { get; set; }
        }

        public class Source
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Article
        {
            public Source source { get; set; }
            public string author { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string urlToImage { get; set; }
            public DateTime publishedAt { get; set; }
            public string content { get; set; }
        }

        public class NewsResponse_root
        {
            public string status { get; set; }
            public int totalResults { get; set; }
            public List<Article> articles { get; set; }
        }
    }
}
