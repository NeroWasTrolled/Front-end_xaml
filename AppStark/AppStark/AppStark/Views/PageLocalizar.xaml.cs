using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using MySqlConnector;
using AppStark.Controller;
using AppStark.Models;

namespace AppStark.Views
{
    public partial class PageLocalizar : ContentPage
    {
        public PageLocalizar()
        {
            InitializeComponent();
            AtualizaLista();
        }

        public void AtualizaLista()
        {
            string cpf = txtCPF.Text;
            BdIndustrias funcionario = new BdIndustrias();
            if (swFavorito.IsToggled)
            {
                ListaFuncionarios.ItemsSource = BdIndustrias.LocalizarFuncionarios(cpf, true);
            }
            else
            {
                ListaFuncionarios.ItemsSource = BdIndustrias.LocalizarFuncionarios(cpf);
            }
        }

        private void btnLocalizar_Clicked(object sender, EventArgs e)
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

        private void swFavorito_Toggled(object sender, ToggledEventArgs e)
        {
            AtualizaLista();
        }

        private void ListaFuncionarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ModIndustrias nota = (ModIndustrias)ListaFuncionarios.ItemsSource;
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageCadastrar(nota));
        }
    }
}
