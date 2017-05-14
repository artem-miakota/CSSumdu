using CSSumdu.Model;
using CSSumdu.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CSSumdu.View
{
    public sealed partial class ScheduleViewPage : Page
    {
        public ScheduleViewPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var eventList = await DB.Instance.getConnetion().Table<Event>().ToListAsync();
            eventList.Sort((x, y) => x.START_TIME.CompareTo(y.START_TIME));

            ObservableCollection<Event> bindedList = new ObservableCollection<Event>(eventList);
            ListBox1.DataContext = bindedList;
        }
    }
}
