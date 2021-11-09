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
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace wakeupapp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        public NewsPage()
        {
            this.InitializeComponent();
            my_news_button_Click(this, new RoutedEventArgs());
        }

        private async void my_news_button_Click(object sender, RoutedEventArgs e)
        {
            var flag = await News.GetArticlesCustom();
            if(flag == false)
            {
                handle_error_msg("Please specify your News topics in Settings");
                return;
            }
            transform_news(News.news_articles_custom);
        }

        private async void news_button_Click(object sender, RoutedEventArgs e)
        {
            var flag = await News.GetArticlesMain();
            transform_news(News.news_articles);
        }

        private async void handle_error_msg(string msg)
        {
            this.error_msg.Visibility = Visibility.Visible;
            this.error_msg.Text = msg;
            await System.Threading.Tasks.Task.Delay(1500);
            this.error_msg.Visibility = Visibility.Collapsed;
            this.error_msg.Text = "";
        }

        private void transform_news(List<News.Article> articles)
        {
            List<TransformedNews> news = new List<TransformedNews>();
            for (int i = 0; i < 15; i++)
            {
                news.Add(new TransformedNews()
                {
                    imageData = new BitmapImage(new Uri(articles[i].urlToImage, UriKind.Absolute)),
                    author = articles[i].author,
                    titleAndDate = String.Format("{0}\n{1}",
                        articles[i].title,
                        articles[i].publishedAt.ToString()),
                    description = articles[i].description
                });
            }
            this.NewsListOptions.ItemsSource = news;
        }

        public class TransformedNews
        {
            public BitmapImage imageData { get; set; }
            public string author { get; set; }
            public string titleAndDate { get; set; }
            public string description { get; set; }
            
        }
    }
}
