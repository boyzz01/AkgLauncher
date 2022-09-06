using Hi3Helper.Data;
using Hi3Helper.Http;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Hi3Helper.Shared.Region.LauncherConfig;
using static Hi3Helper.Locale;
using Microsoft.UI.Windowing;
using Windows.UI.ViewManagement;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;

namespace CollapseLauncher.Pages
{

    public class User
    {
        public string gameProvider { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    public sealed partial class LoginPage : Page
    {
        static HttpClient client = new HttpClient();
        private const string URL = "https://staging-api.troves.gg/v1/api/";
        private string urlParameters = "auth/login";
        private MainPage Instance;

        public LoginPage()
        {
            this.InitializeComponent();
            client.BaseAddress = new Uri(URL);

            //if (readSetting("username") != null)
            //{
            //    emailLoginTxt.Text = readSetting("username");
            //    passwordLoginTxt.Password = readSetting("password");
            //}
        }

        public async void CheckLogin(object sender, RoutedEventArgs e)
        {

            var person = new User();
            person.gameProvider = "";
            person.email = "ardi@akggames.com";
            person.password = "12345678";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(urlParameters, data);

            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Your response data is: " + customerJsonString);

             
                //if (Instance == null)
                //{
                //    Instance = new MainPage();
                //}
                //this.Content = Instance;

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

        public void RegisterTxtClick(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Collapsed;
            register.Visibility = Visibility.Visible;
        }

        public void LoginTxtClick(object sender, RoutedEventArgs e)
        {
            register.Visibility = Visibility.Collapsed;
            login.Visibility = Visibility.Visible;
        }

        private void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            //saveSetting("username", emailLoginTxt.Text);
            //saveSetting("password", passwordLoginTxt.Password.ToString());
            //CheckLogin();
            if (Instance == null)
            {
                Instance = new MainPage();
            }
            this.Content = Instance;
        }

        private void saveSetting(string settingName, string settingValue)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Saving your setting  
            localSettings.Values[settingName] = settingValue;
        }

        //Retrieve your setting value
        private string readSetting(string settingName)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Reading and returning your setting value
            var value = localSettings.Values[settingName];
            if (value != null)
                return value.ToString();
            else
                return "";
        }


    }
}
