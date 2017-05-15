using CSSumdu.Model;
using CSSumdu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CSSumdu.View
{
    public sealed partial class ScheduleQueryPage : Page
    {
        public ScheduleQueryPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            end.Date = start.Date.AddDays(7);

            var eventList = await DB.Instance.getConnetion().Table<Event>().ToListAsync();
            eventList.Sort((x, y) => x.START_TIME.CompareTo(y.START_TIME));

            ObservableCollection<Event> bindedList = new ObservableCollection<Event>(eventList);
            ListBox.DataContext = bindedList;
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            List<String> myList = new List<String>();
            var groupList = await DB.Instance.getConnetion().QueryAsync<Pair>("SELECT * FROM " + sender.Tag + " WHERE name LIKE \"%" + sender.Text + "%\"");
            foreach (var group in groupList)
            {
                myList.Add(group.name);
            }
            sender.ItemsSource = myList;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            b.Focus(FocusState.Programmatic);
        }

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as AutoSuggestBox).Text == "")
            {
                AutoSuggestBox_TextChanged(sender as AutoSuggestBox, null);
            }
        }

        private class Pair
        {
            public int id { get; set; }
            public String name { get; set; }
        }

        private async void b_Click(object sender, RoutedEventArgs e)
        {
            StatusBarProgressIndicator progressbar = StatusBar.GetForCurrentView().ProgressIndicator;
            
            int group = (gr.Text == "") ? 0 : (await DB.Instance.getConnetion().QueryAsync<Pair>("SELECT id FROM groups WHERE name=\"" + gr.Text + "\" LIMIT 1;"))[0].id;
            int teacher = (te.Text == "") ? 0 : (await DB.Instance.getConnetion().QueryAsync<Pair>("SELECT id FROM teachers WHERE name=\"" + te.Text + "\" LIMIT 1;"))[0].id;
            int auditor = (au.Text == "") ? 0 : (await DB.Instance.getConnetion().QueryAsync<Pair>("SELECT id FROM auditoriums WHERE name=\"" + au.Text + "\" LIMIT 1;"))[0].id;
            if (group != 0 | teacher != 0 | auditor != 0)
            {
                await progressbar.ShowAsync();
                await Schedule.Instance.getSchedule(group, teacher, auditor, start.Date, end.Date);

                var eventList = await DB.Instance.getConnetion().Table<Event>().ToListAsync();
                eventList.Sort((x, y) => x.START_TIME.CompareTo(y.START_TIME));

                ObservableCollection<Event> bindedList = new ObservableCollection<Event>(eventList);
                ListBox.DataContext = bindedList;

                await progressbar.HideAsync();
            }



        }
    }
}
