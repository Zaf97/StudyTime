using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace studytime
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Statistics_Page : Page
    {
        int stse = 0;
        int brse = 0;
        string day = "";

        public Statistics_Page()
        {
            this.InitializeComponent();
            

        }

        private void Login_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login_Page));
        }

        private void About_appbarbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_appbarbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginuserlist = new ObservableCollection<Item>(await App.MobileService.GetTable<Item>().ToListAsync());
                
                    foreach (Item t in loginuserlist)
                    {
                        if (Login_Page.username == t.Username)
                        {
                            switch (t.Date)
                            {
                                case "Monday":
                                Monday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;
                                case "Tuesday":
                                    Tuesday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;
                                case "Wednseday":
                                    Wednseday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;
                                case "Thursday":
                                    Thursday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;
                                case "Friday":
                                    Friday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;
                                case "Saturday":
                                    Saturday.Text = t.Study_Sessions.ToString() + "sessions";
                                    break;

                            }
                        }
                    }
            }
            catch { }
        }
    }
}
