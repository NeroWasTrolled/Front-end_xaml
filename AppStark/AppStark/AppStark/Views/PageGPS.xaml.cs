using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppStark.Controller;
using AppStark.Models;
using System.Text.RegularExpressions;

namespace AppStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGPS : ContentPage
    {
        public PageGPS()
        {
            InitializeComponent();
            AtualizaLista();
        }

        public void AtualizaLista()
        {
                String cpf = !string.IsNullOrEmpty(txtCPF.Text) ? txtCPF.Text : "";
                ListarEndereco.ItemsSource = EnderecoFavorito.IsToggled ? BdIndustrias.LocalizarFuncionarios(cpf, true) : BdIndustrias.LocalizarFuncionarios(cpf);
        }

        private void EnderecoFavorito_Toggled(object sender, ToggledEventArgs e)
        {
            AtualizaLista();
        }

        private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (cleanText.Length == 11)
            {
                var formattedText = String.Format("{0:000\\.000\\.000\\-00}", long.Parse(cleanText));

                entry.Text = formattedText;
            }
            else if (cleanText.Length > 11)
            {
                entry.Text = cleanText.Substring(0, 11);
            }
            else
            {
                entry.Text = newText;
            }
            entry.CursorPosition = entry.Text.Length;
        }

        private void btnLocalizar_Clicked(object sender, EventArgs e)
        {
            AtualizaLista();
        }

        private void ListarEndereco_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ModIndustrias funcionario = (ModIndustrias)ListarEndereco.SelectedItem;
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            string enderecoCompleto = $"{funcionario.Endereco}, {funcionario.Bairro}, {funcionario.NumeroCasa}";
            p.Detail = new NavigationPage(new PageGPSEndereco(funcionario, enderecoCompleto));
        }
    }
}