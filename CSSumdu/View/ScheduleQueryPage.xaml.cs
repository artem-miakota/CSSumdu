using CSSumdu.Model;
using CSSumdu.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
    }
}
