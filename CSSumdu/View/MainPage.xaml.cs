using CSSumdu.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CSSumdu
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            DB db = new DB();
            Schedule sc = new Schedule();

            await db.init();
            
            Task[] tasks = new Task[3];
            tasks[0] = sc.getList("http://schedule.sumdu.edu.ua/index/json?method=getGroups", "INSERT INTO groups (id, name) VALUES");
            tasks[1] = sc.getList("http://schedule.sumdu.edu.ua/index/json?method=getTeachers", "INSERT INTO teachers (id, name) VALUES");
            tasks[2] = sc.getList("http://schedule.sumdu.edu.ua/index/json?method=getAuditoriums", "INSERT INTO auditoriums (id, name) VALUES");

            await Task.WhenAll(tasks);
        }
    }
}
