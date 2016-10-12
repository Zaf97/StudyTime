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
using Windows.Phone.Devices.Notification;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Messaging;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
using System.Collections.ObjectModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace studytime
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public MainPage()
        {
            this.InitializeComponent();

            

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            
        }

        
        int totalsec = 0;
        bool Break = false;
               
        

        public async void Timer_Tick(object sender, object e)
        {
            if (Login_Page.userIn)
            {
                mainreg.Visibility = Visibility.Collapsed;
            }
            
            totalsec++;
            if(TimeSpan.FromSeconds(totalsec) == TimeSpan.FromMinutes(MainPage.localSettings.Values["Time"] == null ? Settings_Page.Time : Convert.ToInt32(MainPage.localSettings.Values["Time"])) && Break == false)
            {
                if (Login_Page.userIn)
                {
                    Item newsubject = new Item();
                    newsubject.Username = Login_Page.username;
                    newsubject.Subject = Subject_box.Text;
                    newsubject.Study_Sessions = 1;
                    newsubject.Break_Sessions = 0;
                    newsubject.Date = DateTime.Today.DayOfWeek.ToString();
                    newsubject.Password = null;
                    newsubject.Time = Settings_Page.Time;
                    newsubject.Break_Time = Settings_Page.BreakTime;

                    try
                    {
                        var loginuserlist = new ObservableCollection<Item>(await App.MobileService.GetTable<Item>().ToListAsync());

                        foreach (var c in loginuserlist)
                        {
                            if (newsubject.Username == c.Username && c.Password == null)
                            {
                                if (newsubject.Date == c.Date)
                                {
                                    newsubject.Id = c.Id;
                                    newsubject.Study_Sessions = c.Study_Sessions + 1;
                                    await App.MobileService.GetTable<Item>().UpdateAsync(newsubject);
                                }
                                else
                                {
                                    await App.MobileService.GetTable<Item>().InsertAsync(newsubject);

                                }
                            }

                            
                        }
                    }
                    catch { }
                }

                
                MainGrid.Background = new SolidColorBrush(Windows.UI.Colors.GreenYellow);
                Break = true;    
                totalsec = 0;
                var dialog = new Windows.UI.Popups.MessageDialog("Time to take a break!");
                await dialog.ShowAsync();
            }
            else if(TimeSpan.FromSeconds(totalsec) == TimeSpan.FromMinutes(Settings_Page.BreakTime) && Break == true)
            {
                if (Login_Page.userIn)
                {
                    Item newsubject = new Item();
                    newsubject.Username = Login_Page.username;
                    newsubject.Date = DateTime.Today.ToString();

                    try
                    {
                        var loginuserlist = new ObservableCollection<Item>(await App.MobileService.GetTable<Item>().ToListAsync());

                        foreach (var c in loginuserlist)
                        {
                            if (Login_Page.username == c.Username)
                            {

                                if (newsubject.Date == c.Date)
                                {
                                    newsubject.Break_Sessions = c.Break_Sessions++;
                                    await App.MobileService.GetTable<Item>().UpdateAsync(newsubject);
                                }

                            }
                        }
                    }
                    catch { }
                }
                MainGrid.Background = new SolidColorBrush(Windows.UI.Colors.Tomato);
                Break = false;
                totalsec = 0;
                var dialog = new Windows.UI.Popups.MessageDialog("Get back to work!");
                await dialog.ShowAsync();
            }
            else if(Break == false)
            {
                TimeSpan available_type = TimeSpan.FromMinutes(MainPage.localSettings.Values["Time"] == null ? Settings_Page.Time : Convert.ToInt32(MainPage.localSettings.Values["Time"])) - TimeSpan.FromSeconds(totalsec);
                string time = available_type.Minutes.ToString() + ":" + (available_type.Seconds % 60).ToString("00");
                timer_box.Text = time;
            }
            else
            {
                TimeSpan available_type = TimeSpan.FromMinutes(Settings_Page.BreakTime) - TimeSpan.FromSeconds(totalsec);
                string breaktime = available_type.Minutes.ToString() + ":" + (available_type.Seconds % 60).ToString("00");
                timer_box.Text = breaktime;
            }
        }

        
        

        private void Settingbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings_Page));            
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            startBtn.Visibility = Visibility.Collapsed;
            stopBtn.Visibility = Visibility.Visible;
            TimerImg.Visibility = Visibility.Collapsed;
            timer_box.Visibility = Visibility.Visible;
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            totalsec = 0;
            if (Break)
            {
                MainGrid.Background = new SolidColorBrush(Windows.UI.Colors.Tomato);
                Break = false;
            }
            startBtn.Visibility = Visibility.Visible;
            stopBtn.Visibility = Visibility.Collapsed;
            TimerImg.Visibility = Visibility.Visible;
            timer_box.Visibility = Visibility.Collapsed;

        }

        private void Subject_TextChanged(object sender, TextChangedEventArgs e)
        {
            localSettings.Values["Subject"] = Subject_box.Text;
            
            
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login_Page));
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Statistics_Page));

        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About_Page));
        }
    }

    public class Item
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Subject{ get; set; }
        public int Time { get; set; }
        public int Study_Sessions { get; set; }
        public int Break_Time { get; set; }
        public int Break_Sessions { get; set; }
        public string Date { get; set; }
    }

   


}
