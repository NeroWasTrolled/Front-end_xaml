using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace AppStark.Models
{
    public class ModIndustrias
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public String CPF { get; set; }
        public String RG { get; set; }
        public String Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public String Endereco { get; set; }
        public String Bairro { get; set; }
        public String Complemento { get; set; }
        public String NumeroCasa { get; set; }
        public String Email { get; set; }
        public String Setor { get; set; }
        public String Turno { get; set; }
        public byte[] Foto { get; set; }
        public Boolean Favorito { get; set; }
        public String Dados { get; set; }  
    }
}
