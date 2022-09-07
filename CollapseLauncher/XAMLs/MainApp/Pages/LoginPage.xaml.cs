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
        public string otp { get; set; }
        public string username { get; set; }
        public string provider { get; set; }

    }

    public class ResponseOtp
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public object error { get; set; }
    }

    public class Data
    {
        public bool valid { get; set; }
    }

    public sealed partial class LoginPage : Page
    {
        public Timer timer;
        public int counter = 0;
        public string msg = "";
        static HttpClient client = new HttpClient();
        private const string URL = "https://staging-api.troves.gg/v1/api/";
        //private string urlParameters = "auth/login";
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

        private async void ContinueBtnClick(object sender, RoutedEventArgs e)
        {
            if (passwordBoxRegister.Password.ToString().Length < 8)
            {
                errorTxt2.Text = "Password Length Min 8";
                errorTxt2.Visibility = Visibility.Visible;
                timer = new Timer(TimerTick, null, 2000, 2000);
            }else
            if (passwordBoxRegister.Password.ToString() != passwordBox2Register.Password.ToString())
            {
                errorTxt2.Text = "Password do not match!";
                errorTxt2.Visibility = Visibility.Visible;
                timer = new Timer(TimerTick, null, 1000, 1000);
            }
            else
            {
                var person = new User();
                person.email = emailRegister.Text.ToString();
         

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
                var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("auth/send_otp", data);

                if (response.IsSuccessStatusCode)
                {
                    var customerJsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Your response data is: " + customerJsonString);
                    register.Visibility = Visibility.Collapsed;
                    otp.Visibility = Visibility.Visible;
                    emailOtp.Text = emailRegister.Text;

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
           

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
           // var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
           // var response = await client.PostAsync(urlParameters, data);

        }

        public void TimerTick(Object stateInfo)
        {
            DispatcherQueue.TryEnqueue(() => { errorTxt2.Visibility = Visibility.Collapsed; });
         
        }
        private async void SubmitBtnClick(object sender, RoutedEventArgs e)
        {
            //saveSetting("username", emailLoginTxt.Text);
            //saveSetting("password", passwordLoginTxt.Password.ToString());
            //CheckLogin();
            var person = new User();
            person.email = emailRegister.Text.ToString();
            person.otp = otpTxt.Text.ToString();


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("auth/check_otp", data);


            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Your response data is: " + customerJsonString);
                register.Visibility = Visibility.Collapsed;
                otp.Visibility = Visibility.Visible;
                emailOtp.Text = emailRegister.Text;

                var res = new ResponseOtp();

                res = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseOtp>(customerJsonString);

                if (res.data.valid == true)
                {
                    Console.WriteLine("Your response data is: " + customerJsonString);
                    var user = new User();
                    user.email = emailRegister.Text.ToString();
                    user.password = passwordBoxRegister.Password.ToString();
                    user.otp = otpTxt.Text.ToString();
                    user.username = emailRegister.Text.ToString();
                    user.provider = "LOCAL";

                    var jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                    var dataUser = new System.Net.Http.StringContent(jsonUser, Encoding.UTF8, "application/json");
                    var responseUser = await client.PostAsync("auth/users/register", dataUser);

                    if (responseUser.IsSuccessStatusCode)
                    {
                        var resString = await responseUser.Content.ReadAsStringAsync();
                        Console.WriteLine("Your response data is: " + resString);
                        otp.Visibility = Visibility.Collapsed;
                        login.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)responseUser.StatusCode, responseUser.ReasonPhrase);
                    }
                }

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


        private async void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            //saveSetting("username", emailLoginTxt.Text);
            //saveSetting("password", passwordLoginTxt.Password.ToString());
            //CheckLogin();
            var person = new User();
            person.gameProvider = "blueprotocol";
            person.email = emailLoginTxt.Text;
            person.password = passwordLoginTxt.Password.ToString();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("auth/login", data);

            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Your response data is: " + customerJsonString);


                if (Instance == null)
                {
                    Instance = new MainPage();
                }
                this.Content = Instance;

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

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
