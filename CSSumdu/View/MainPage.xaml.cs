using CSSumdu.View;
using CSSumdu.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CSSumdu
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await DB.Instance.init();

            await Schedule.Instance.getSchedule(100807, 0, 0, new DateTimeOffset(2017, 1, 1, 0, 0, 0, new TimeSpan()), new DateTimeOffset(2017, 4, 1, 0, 0, 0, new TimeSpan()));

            Task[] tasks = new Task[3];
            tasks[0] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getGroups", "INSERT INTO groups (id, name) VALUES");
            tasks[1] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getTeachers", "INSERT INTO teachers (id, name) VALUES");
            tasks[2] = Schedule.Instance.getList("http://schedule.sumdu.edu.ua/index/json?method=getAuditoriums", "INSERT INTO auditoriums (id, name) VALUES");

            await Task.WhenAll(tasks);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScheduleQueryPage));
        }
    }
}
