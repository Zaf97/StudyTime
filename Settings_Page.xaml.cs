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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace studytime
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings_Page : Page

    {
        public static int Time = 45;
        public static int BreakTime = 5;

        public Settings_Page()
        {
            this.InitializeComponent();
            try
            {
                setTime.Text = MainPage.localSettings.Values["Time"].ToString();
                setTime.Text = MainPage.localSettings.Values["BreakTime"].ToString();
                Time = (int)MainPage.localSettings.Values["Time"];
                BreakTime = (int)MainPage.localSettings.Values["BreakTime"];
            }
            catch { }
        }

        

        public async void setTime_changed(object e, TextChangedEventArgs arg)
        {
            if (!int.TryParse(setTime.Text, out Time) && setTime.Text.Length == 0 )
            {
                var dialog = new Windows.UI.Popups.MessageDialog("It needs to be an integer!");
                await dialog.ShowAsync();
                return;
            }
            MainPage.localSettings.Values["Time"] = Time;

        }

       
        private async void setBreakTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (!int.TryParse(setBreakTime.Text, out BreakTime))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("It needs to be an integer!");
                await dialog.ShowAsync();
                return;
            }
            MainPage.localSettings.Values["BreakTime"] = BreakTime;
        }

        private void Login_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login_Page));
        }

        private void Statistics_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Statistics_Page));

        }

        private void About_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About_Page));

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(BreakTime > 0 && Time > 0)
            {
                Frame.Navigate(typeof(MainPage));
            }else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("It needs to be an integer!");
                await dialog.ShowAsync();
            }
        }
    }
}
