using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppStark.Effects;

namespace AppStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSobre : ContentPage
    {
        public PageSobre()
        {
            InitializeComponent();
            PageTitleHelper.HideTitle(this);
        }
    }
}