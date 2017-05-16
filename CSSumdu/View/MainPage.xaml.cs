﻿using CSSumdu.View;
using CSSumdu.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

namespace CSSumdu
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            addItems();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await DB.Instance.init();
            await GetSSUNews();

            Task[] tasks = new Task[3];
            tasks[0] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getGroups", "INSERT INTO groups (id, name) VALUES");
            tasks[1] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getTeachers", "INSERT INTO teachers (id, name) VALUES");
            tasks[2] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getAuditoriums", "INSERT INTO auditoriums (id, name) VALUES");
            await Task.WhenAll(tasks);
        }

        private async Task GetSSUNews()
        {
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri("http://sumdu.edu.ua/ukr/component/content/category/12-news.feed"));
            if (feed != null)
            {
                foreach (SyndicationItem item in feed.Items)
                {
                    item.Summary.Text = item.Summary.Text.Split('"')[5];
                    flipView.Items.Add(item);
                }
            }
        }

        private class MenuItem
        {
            public String header { get; set; }
            public Type type { get; set; }
        }

        private void mainView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as MenuItem;
            if (item != null)
            {
                Frame.Navigate(item.type);
            }
        }

        private void addItems()
        {
            mainView.Items.Add(new MenuItem()
            {
                header = "Розклад",
                type = typeof(ScheduleQueryPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Інформація про спеціальності",
                type = typeof(SpecialtyPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Історія кафедри",
                type = typeof(HistoryPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Науково-дослідна робота",
                type = typeof(ResearchPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Міжнародне співробітництво",
                type = typeof(InternationalPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Структурні підрозділи кафедри",
                type = typeof(SubdivisionsPage),
            });

            mainView.Items.Add(new MenuItem()
            {
                header = "Проходження практики",
                type = typeof(PracticePage),
            });

        }
    }
}
