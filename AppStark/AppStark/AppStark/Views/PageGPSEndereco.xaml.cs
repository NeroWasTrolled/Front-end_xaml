using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppStark.Models;
using System.Linq;

namespace AppStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGPSEndereco : ContentPage
    {
        private ModIndustrias funcionario;

        private string address;

        public PageGPSEndereco(ModIndustrias funcionario, string address)
        {
            InitializeComponent();
            this.funcionario = funcionario;
            this.address = address;
            txtEndereco.Text = funcionario.Endereco;
            txtBairro.Text = funcionario.Bairro;
            txtNumeroDacasa.Text = funcionario.NumeroCasa;
        }

        public async Task MostrarNoMapa()
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest()
            { DesiredAccuracy = GeolocationAccuracy.Best });
            var locationinfo = new Location(location.Latitude, location.Longitude);
            var options = new MapLaunchOptions { Name = "Localização do seu Funcionário" };
            await Map.OpenAsync(locationinfo, options);
        }

        private async void btnMapa_Clicked(object sender, EventArgs e)
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(this.address);

                if (locations != null && locations.Any())
                {
                    var location = locations.FirstOrDefault();
                    var options = new MapLaunchOptions { Name = "Localização do seu Funcionário" };
                    await Map.OpenAsync(location.Latitude, location.Longitude, options);
                }
                else
                {
                    await DisplayAlert("Aviso", "Não foi possível encontrar a localização do endereço.", "OK");
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Aviso", "Seu dispositivo não suporta o recurso de mapa.", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Aviso", "É necessário permitir o acesso à sua localização para usar o recurso de mapa.", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao tentar abrir o mapa: {ex.Message}", "Ok");
            }
        }

        private void txtNumeroDacasa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (string.IsNullOrEmpty(newText))
            {
                entry.Text = "N°";
            }
            else if (newText != cleanText)
            {
                entry.Text = "N°" + cleanText;
            }
        }
    }
}
