using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();
            AnimateProgressBar();
        }

        public async void AnimateProgressBar()
        {
            for (int i = 0; i <= 100; i++)
            {
                progressBar.Progress = i / 100.0;
                progressLabel.Text = $"{i}%";

                await Task.Delay(10);
            }

            await Task.Delay(1000);
            await Navigation.PopModalAsync();
        }
    }
}