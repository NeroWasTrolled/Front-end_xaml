using AppStark.Views;
using SeuNamespace;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AppStark
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new PagePrincipal();
            Resources.Add("ByteArrayToImageSourceConverter", new ByteArrayToImageSourceConverter());
        }

        protected override async void OnStart()
        {
            MainPage = new SplashScreen();
            await Task.Delay(3000);
            MainPage = new NavigationPage(new PagePrincipal());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
