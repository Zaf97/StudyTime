using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class Login_Page : Page
    {
        public Login_Page()
        {
            this.InitializeComponent();

        }

        public static string username;
        public static bool userIn = false;
        bool exist = false;

        private async void Registerbtn_Click(object sender, RoutedEventArgs e)
        {
            if(Username_box.Text!= null && Password_box.Password != null)
            {
                Item newUser = new Item();
                newUser.Username = Username_box.Text;
                newUser.Password = Password_box.Password;
                try
                {
                    var itemlist = new ObservableCollection<Item>(await App.MobileService.GetTable<Item>().ToListAsync());
                    foreach (var c in itemlist)
                    {
                        if (newUser.Username == c.Username)
                        {
                            exist = true;
                        }

                    }
                    if(!exist)
                    {
                        await App.MobileService.GetTable<Item>().InsertAsync(newUser);
                        username = Username_box.Text;
                        userIn = true;
                        
                        Frame.Navigate(typeof(MainPage));

                    }
                    else
                    {
                        var dialog = new Windows.UI.Popups.MessageDialog(
                        "This username is not available.");
                        await dialog.ShowAsync();
                    }
                }
                catch(Exception ex)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog(
                        "Sorry something went wrong. Check your internet connection and try again.");
                    await dialog.ShowAsync();
                    Debug.WriteLine(ex.ToString());
                }

            }
        }

        private async void Loginbtn_Click(object sender, RoutedEventArgs e)
        {
            var userslist = new ObservableCollection<Item>(await App.MobileService.GetTable<Item>().ToListAsync());
            foreach(var t in userslist)
            {
                if(Username_box.Text == t.Username && Password_box.Password == t.Password)
                {
                    username = t.Username;
                    userIn = true;
                    Frame.Navigate(typeof(MainPage));
                }
            }
        }

        private void Settings_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings_Page));
        }

        private void Statistics_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Statistics_Page));

        }

        private void About_appbarbtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About_Page));

        }
    }
}
