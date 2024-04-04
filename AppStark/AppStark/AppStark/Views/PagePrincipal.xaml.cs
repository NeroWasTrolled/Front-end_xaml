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
    public partial class PagePrincipal : MasterDetailPage
    {
        public PagePrincipal()
        {
            InitializeComponent();
            btnHome_Clicked(null, EventArgs.Empty);
        }

        private void btnHome_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PageHome());
            IsPresented = false;
        }

        private void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PageCadastrar());
            IsPresented = false;
        }

        private void btnlocalizar_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PageLocalizar());
            IsPresented = false;
        }

        private void btnlocalizarCasa_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PageGPS());
            IsPresented = false;
        }

        private void btnsobre_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new PageSobre());
            IsPresented = false;
        }
    }
}