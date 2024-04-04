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
    public partial class PageHome : ContentPage
    {
        public PageHome()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Page currentPage = Application.Current.MainPage;
            if (currentPage is NavigationPage navigationPage)
            {
                Page currentPageInsideNavigation = navigationPage.CurrentPage;

                if (currentPageInsideNavigation is MasterDetailPage masterDetailPage)
                {
                    PageCadastrar pageCadastrar = new PageCadastrar();

                    NavigationPage newNavigationPage = new NavigationPage(pageCadastrar);

                    masterDetailPage.Detail = newNavigationPage;

                    masterDetailPage.IsPresented = false;
                }
                else
                {
                    await currentPage.DisplayAlert("Erro", "Não foi possível encontrar a MasterDetailPage.", "OK");
                }
            }
            else
            {
                await currentPage.DisplayAlert("Erro", "Não foi possível encontrar a NavigationPage.", "OK");
            }
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Page currentPage = Application.Current.MainPage;
            if (currentPage is NavigationPage navigationPage)
            {
                Page currentPageInsideNavigation = navigationPage.CurrentPage;

                if (currentPageInsideNavigation is MasterDetailPage masterDetailPage)
                {
                    PageLocalizar pageLocalizar = new PageLocalizar();

                    NavigationPage newNavigationPage = new NavigationPage(pageLocalizar);

                    masterDetailPage.Detail = newNavigationPage;

                    masterDetailPage.IsPresented = false;
                }
                else
                {
                    await currentPage.DisplayAlert("Erro", "Não foi possível encontrar a MasterDetailPage.", "OK");
                }
            }
            else
            {
                await currentPage.DisplayAlert("Erro", "Não foi possível encontrar a NavigationPage.", "OK");
            }
        }

    }
}