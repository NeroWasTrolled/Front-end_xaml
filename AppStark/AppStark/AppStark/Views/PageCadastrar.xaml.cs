using AppStark.Models;
using AppStark.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Text.RegularExpressions;
using MySqlConnector;

namespace AppStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCadastrar : ContentPage
    {
        public PageCadastrar(ModIndustrias funcionario)
        {
            InitializeComponent();
            btnSalvar.Text = "Alterar";
            txtCodigo.IsVisible = true;
            btnExcluir.IsVisible = true;
            txtCodigo.Text = funcionario.Id.ToString();
            txtNome.Text = funcionario.Nome;
            txtCPF.Text = funcionario.CPF;
            txtRG.Text = funcionario.RG;
            txtTelefone.Text = funcionario.Telefone;
            txtEndereco.Text = funcionario.Endereco;
            txtBairro.Text = funcionario.Bairro;
            txtComplemento.Text = funcionario.Complemento;
            txtNumeroDacasa.Text = funcionario.NumeroCasa;
            txtEmail.Text = funcionario.Email;
            pkrSetor.SelectedItem = funcionario.Setor?.ToString();
            pkrTurno.SelectedItem = funcionario.Turno?.ToString();
            txtDados.Text = funcionario.Dados;
            swFavorito.IsToggled = funcionario.Favorito;

            if (funcionario.Foto != null && funcionario.Foto.Length > 0)
            {
                imgFoto.Source = ImageSource.FromStream(() => new MemoryStream(funcionario.Foto));
            }
            else
            {
                imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
            }
        }

        public PageCadastrar()
        {
            InitializeComponent();
        }

        private async void btnTirarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var foto = await TirarFotoAsync();

                if (foto != null)
                {
                    var nomeArquivo = Path.Combine(FileSystem.AppDataDirectory, "foto.jpg");
                    using (var destino = File.OpenWrite(nomeArquivo))
                    {
                        await destino.WriteAsync(foto, 0, foto.Length);
                    }
                    imgFoto.Source = ImageSource.FromStream(() => new MemoryStream(foto));
                }
                else
                {
                    imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task<byte[]> TirarFotoAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Erro", "A permissão para acessar a câmera não foi concedida.", "OK");
                return null;
            }

            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto == null)
            {
                await DisplayAlert("Erro", "Nenhuma foto foi capturada.", "OK");
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var stream = await foto.OpenReadAsync();
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var nomeArquivo = Path.Combine(FileSystem.AppDataDirectory, "foto.jpg");
                byte[] foto = null;

                if (imgFoto.Source != null)
                {
                    foto = await GetFotoBytes(imgFoto.Source);
                    if (foto != null)
                    {
                        using (var destino = File.OpenWrite(nomeArquivo))
                        {
                            await destino.WriteAsync(foto, 0, foto.Length);
                        }
                        imgFoto.Source = ImageSource.FromStream(() => new MemoryStream(foto));
                    }
                    else
                    {
                        imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
                    }
                }

                ModIndustrias funcionario = new ModIndustrias();
                funcionario.Nome = txtNome.Text;
                funcionario.CPF = txtCPF.Text;
                funcionario.RG = txtRG.Text;
                funcionario.Telefone = txtTelefone.Text;
                funcionario.DataNascimento = dpNascimento.Date;
                funcionario.Endereco = txtEndereco.Text;
                funcionario.Bairro = txtBairro.Text;
                funcionario.Complemento = txtComplemento.Text;
                funcionario.NumeroCasa = txtNumeroDacasa.Text;
                funcionario.Email = txtEmail.Text;
                funcionario.Setor = pkrSetor.SelectedItem?.ToString();
                funcionario.Turno = pkrTurno.SelectedItem?.ToString();
                funcionario.Favorito = swFavorito.IsToggled;
                funcionario.Dados = txtDados.Text;
                funcionario.Foto = foto;

                if (dpNascimento.Date != null)
                {
                    if (!IsMaiorDeIdade(dpNascimento.Date))
                    {
                        await DisplayAlert("Erro", "Você precisa ter mais de 21 anos para ser cadastrado.", "OK");
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Por favor, selecione a data de nascimento.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(funcionario.Nome) || string.IsNullOrEmpty(funcionario.CPF) || string.IsNullOrEmpty(funcionario.Telefone))
                {
                    await DisplayAlert("Erro", "Por favor, preencha os campos obrigatórios.", "OK");
                    return;
                }

                BdIndustrias dBIndustrias = new BdIndustrias();
                if (btnSalvar.Text == "Inserir")
                {
                    BdIndustrias.InserirFuncionario(funcionario);
                    await DisplayAlert("Resultado", BdIndustrias.StatusMessage, "OK");
                    Application.Current.MainPage = new MasterDetailPage
                    {
                        Master = new PagePrincipal(), 
                        Detail = new NavigationPage(new PageHome())
                    };
                }
                else
                {
                    funcionario.Id = Convert.ToInt32(txtCodigo.Text);
                    BdIndustrias.AlterarFuncionario(funcionario);
                }

                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<byte[]> GetFotoBytes(ImageSource source)
        {
            byte[] foto = null;
            try
            {
                var stream = await ((StreamImageSource)source).Stream(CancellationToken.None);
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    foto = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return foto;
        }

        private async void btnExcluir_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Excluir Funcionários", "Deseja excluir esse funcionário selecionado?", "Sim", "Não");
            if (resp)
            {
                ModIndustrias funcionario = (ModIndustrias)BindingContext;
                int id = funcionario.Id;
                BdIndustrias.ExcluirFuncionario(id);

                await DisplayAlert("Resultado", BdIndustrias.StatusMessage, "OK");

                MySqlConnection conexao = new MySqlConnection();
                BdIndustrias.AtualizarIDsRestantes(conexao, id);

                var p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageHome());
        }

        private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (cleanText.Length == 11)
            {
                var formattedText = string.Format("{0:000\\.000\\.000\\-00}", long.Parse(cleanText));
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

        private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = txtTelefone.Text;

            string numeros = new string(texto.Where(char.IsDigit).ToArray());

            if (numeros.Length >= 11)
            {
                texto = string.Format("({0}) {1}-{2}", numeros.Substring(0, 2), numeros.Substring(2, 5), numeros.Substring(7, 4));
            }

            txtTelefone.Text = texto;
        }

        private void txtNumeroDacasa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (!string.IsNullOrEmpty(cleanText))
            {
                cleanText = "N°" + cleanText;
            }
            entry.Text = cleanText;
        }

        private void txtRG_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (cleanText.Length >= 9)
            {
                var formattedText = String.Format("{0:00\\.000\\.000\\-0}", long.Parse(cleanText));

                entry.Text = formattedText;
            }
            else
            {
                entry.Text = newText;
            }
            entry.CursorPosition = entry.Text.Length;
        }

        private async void dpNascimento_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime selectedDate = e.NewDate;

            if (IsMaiorDeIdade(selectedDate))
            {
                await DisplayAlert("Verificação de Idade", "Você tem mais de 21 anos.", "OK");
                dpNascimento.TextColor = Color.Green;
                imgValidade.IsVisible = true;
                imgValidade.Source = "certo.png";
            }
            else
            {
                await DisplayAlert("Verificação de Idade", "Você tem menos de 21 anos.", "OK");
                dpNascimento.TextColor = Color.Red;
                imgValidade.IsVisible = true;
                imgValidade.Source = "erro.png";
            }
        }

        private bool IsMaiorDeIdade(DateTime dataNascimento)
        {
            int idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Today.AddYears(-idade))
                idade--;

            return idade >= 21;
        }
    }
}
