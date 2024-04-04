using System;
using System.Collections.Generic;
using MySqlConnector;
using System.IO;
using AppStark.Models;
using Xamarin.Forms;
using System.Linq;

namespace AppStark.Controller
{
    public class BdIndustrias
    {
        static string conn = @"Host=sql.freedb.tech;Port=3306;Database=freedb_IndustriasStark;User ID=freedb_IndustriasStark;Password=vz4teVwSN!c6u26;Charset=utf8;";
        public static string StatusMessage { get; set; }

        public static void InserirFuncionario(ModIndustrias funcionario)
        {
            try
            {
                string sql = "INSERT INTO Funcionários(nome,cpf,rg,telefone,datanascimento,endereco,bairro,complemento,numerocasa,email,setor,turno,foto,favorito,dados) " +
                             "VALUES (@nome,@cpf,@rg,@telefone,@datanascimento,@endereco,@bairro,@complemento,@numerocasa,@email,@setor,@turno,@foto,@favorito,@dados)";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                        cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.CPF;
                        cmd.Parameters.Add("@rg", MySqlDbType.VarChar).Value = funcionario.RG;
                        cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                        cmd.Parameters.Add("@datanascimento", MySqlDbType.Date).Value = funcionario.DataNascimento.Date;
                        cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = funcionario.Endereco;
                        cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = funcionario.Bairro;
                        cmd.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = funcionario.Complemento;
                        cmd.Parameters.Add("@numerocasa", MySqlDbType.VarChar).Value = funcionario.NumeroCasa;
                        cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                        cmd.Parameters.Add("@setor", MySqlDbType.VarChar).Value = funcionario.Setor;
                        cmd.Parameters.Add("@turno", MySqlDbType.VarChar).Value = funcionario.Turno;
                        cmd.Parameters.Add("@foto", MySqlDbType.VarBinary).Value = funcionario.Foto;
                        cmd.Parameters.Add("@favorito", MySqlDbType.Bit).Value = funcionario.Favorito;
                        cmd.Parameters.Add("@dados", MySqlDbType.VarChar).Value = funcionario.Dados;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                SalvarFoto(funcionario, funcionario.Foto);
                StatusMessage = $"Funcionário {funcionario.Nome} cadastrado com sucesso !!!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao cadastrar funcionário: {ex.Message}";
                throw;
            }
        }

        public static void AlterarFuncionario(ModIndustrias funcionario)
        {
            try
            {
                string sql = "UPDATE Funcionários SET nome=@nome, cpf=@cpf, rg=@rg, telefone=@telefone, datanascimento=@datanascimento, " +
                             "endereco=@endereco, bairro=@bairro, complemento=@complemento, numerocasa=@numerocasa, email=@email, " +
                             "setor=@setor, turno=@turno, foto=@foto, favorito=@favorito, dados=@dados WHERE id=@id";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                        cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.CPF;
                        cmd.Parameters.Add("@rg", MySqlDbType.VarChar).Value = funcionario.RG;
                        cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                        cmd.Parameters.Add("@datanascimento", MySqlDbType.DateTime).Value = funcionario.DataNascimento.Date;
                        cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = funcionario.Endereco;
                        cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = funcionario.Bairro;
                        cmd.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = funcionario.Complemento;
                        cmd.Parameters.Add("@numerocasa", MySqlDbType.VarChar).Value = funcionario.NumeroCasa;
                        cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                        cmd.Parameters.Add("@setor", MySqlDbType.VarChar).Value = funcionario.Setor;
                        cmd.Parameters.Add("@turno", MySqlDbType.VarChar).Value = funcionario.Turno;
                        cmd.Parameters.Add("@foto", MySqlDbType.VarBinary).Value = funcionario.Foto;
                        cmd.Parameters.Add("@favorito", MySqlDbType.Bit).Value = funcionario.Favorito;
                        cmd.Parameters.Add("@dados", MySqlDbType.VarChar).Value = funcionario.Dados;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = funcionario.Id;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }

                StatusMessage = $"Funcionário {funcionario.Nome} alterado com sucesso !!!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao alterar funcionário: {ex.Message}";
            }
        }

        public static void ExcluirFuncionario(int id)
        {
            try
            {
                string sql = "DELETE FROM Funcionários WHERE id=@id";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    StatusMessage = $"Funcionário com ID {id} excluído com sucesso !!!";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao excluir funcionário: {ex.Message}";
            }
        }

        public static void AtualizarIDsRestantes(MySqlConnection con, int deletedId)
        {
            try
            {
                string updateSql = "UPDATE Funcionários SET id = id - 1 WHERE id > @deletedId";
                using (MySqlCommand updateCmd = new MySqlCommand(updateSql, con))
                {
                    updateCmd.Parameters.AddWithValue("@deletedId", deletedId);
                    updateCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar IDs restantes: {ex.Message}");
            }
        }

        public static List<ModIndustrias> LocalizarFuncionarios(string cpf)
        {
            List<ModIndustrias> listaFuncionarios = new List<ModIndustrias>();

            try
            {
                string sql = "SELECT * FROM Funcionários WHERE cpf = @cpf";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cpf;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModIndustrias funcionario = new ModIndustrias()
                                {
                                    Id = reader.GetInt32("Id"), 
                                    Nome = reader.GetString("Nome"),
                                    CPF = reader.GetString("CPF"),
                                    RG = reader.GetString("RG"),
                                    Telefone = reader.GetString("Telefone"),
                                    DataNascimento = reader.GetDateTime("DataNascimento"),
                                    Endereco = reader.GetString("Endereco"),
                                    Bairro = reader.GetString("Bairro"),
                                    Complemento = reader.GetString("Complemento"),
                                    NumeroCasa = reader.GetString("NumeroCasa"),
                                    Email = reader.GetString("Email"),
                                    Setor = reader.GetString("Setor"),
                                    Turno = reader.GetString("Turno"),
                                    Foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
                                    Favorito = reader.GetBoolean("Favorito"),
                                    Dados = reader.GetString("Dados")
                                };
                                listaFuncionarios.Add(funcionario);
                            }
                        }
                    }
                }

                if (listaFuncionarios.Count > 0)
                {
                    StatusMessage = "Funcionários encontrados.";
                }
                else
                {
                    StatusMessage = "Nenhum funcionário encontrado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao localizar funcionários: {ex.Message}";
            }

            return listaFuncionarios;
        }

        public static List<ModIndustrias> LocalizarFuncionarios(string cpf, Boolean favorito)
        {
            List<ModIndustrias> listaFuncionarios = new List<ModIndustrias>();

            try
            {
                string sql = "SELECT * FROM Funcionários WHERE cpf = @cpf AND favorito = @favorito";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cpf;
                        cmd.Parameters.Add("@favorito", MySqlDbType.Bit).Value = favorito;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModIndustrias funcionario = new ModIndustrias()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nome = reader.GetString("Nome"),
                                    CPF = reader.GetString("CPF"),
                                    RG = reader.GetString("RG"),
                                    Telefone = reader.GetString("Telefone"),
                                    DataNascimento = reader.GetDateTime("DataNascimento"),
                                    Endereco = reader.GetString("Endereco"),
                                    Bairro = reader.GetString("Bairro"),
                                    Complemento = reader.GetString("Complemento"),
                                    NumeroCasa = reader.GetString("NumeroCasa"),
                                    Email = reader.GetString("Email"),
                                    Setor = reader.GetString("Setor"),
                                    Turno = reader.GetString("Turno"),
                                    Foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
                                    Favorito = reader.GetBoolean("Favorito"),
                                    Dados = reader.GetString("Dados")
                                };
                                listaFuncionarios.Add(funcionario);
                            }
                        }
                    }
                }

                if (listaFuncionarios.Count > 0)
                {
                    StatusMessage = "Funcionários encontrados.";
                }
                else
                {
                    StatusMessage = "Nenhum funcionário encontrado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao localizar funcionários: {ex.Message}";
            }

            return listaFuncionarios;
        }

        public static List<ModIndustrias> ListarFuncionarios()
        {
            List<ModIndustrias> listaFuncionarios = new List<ModIndustrias>();

            try
            {
                string sql = "SELECT * FROM Funcionários";
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModIndustrias funcionario = new ModIndustrias()
                                {
                                    Id = reader.GetInt32("Id"),
                                    Nome = reader.GetString("Nome"),
                                    CPF = reader.GetString("CPF"),
                                    RG = reader.GetString("RG"),
                                    Telefone = reader.GetString("Telefone"),
                                    DataNascimento = reader.GetDateTime("DataNascimento"),
                                    Endereco = reader.GetString("Endereco"),
                                    Bairro = reader.GetString("Bairro"),
                                    Complemento = reader.GetString("Complemento"),
                                    NumeroCasa = reader.GetString("NumeroCasa"),
                                    Email = reader.GetString("Email"),
                                    Setor = reader.GetString("Setor"),
                                    Turno = reader.GetString("Turno"),
                                    Foto = reader.IsDBNull(reader.GetOrdinal("foto")) ? null : (byte[])reader["foto"],
                                    Favorito = reader.GetBoolean("Favorito"),
                                    Dados = reader.GetString("Dados")
                                };
                                listaFuncionarios.Add(funcionario);
                            }
                        }
                    }
                }

                if (listaFuncionarios.Count > 0)
                {
                    StatusMessage = "Funcionários encontrados.";
                }
                else
                {
                    StatusMessage = "Nenhum funcionário encontrado.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao listar funcionários: {ex.Message}";
            }

            return listaFuncionarios;
        }

        public static void SalvarFoto(ModIndustrias industria, byte[] fotoBytes)
        {
            if (industria == null)
            {
                throw new ArgumentNullException(nameof(industria), "A indústria não pode ser nula.");
            }

            if (fotoBytes == null || fotoBytes.Length == 0)
            {
                throw new ArgumentException("Os bytes da foto não podem ser nulos ou vazios.", nameof(fotoBytes));
            }

            try
            {
                string caminhoFotos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FotosIndustrias");

                if (!Directory.Exists(caminhoFotos))
                {
                    Directory.CreateDirectory(caminhoFotos);
                }

                string nomeArquivo = $"{industria.Id}.png";
                string caminhoArquivo = Path.Combine(caminhoFotos, nomeArquivo);

                File.WriteAllBytes(caminhoArquivo, fotoBytes);

                industria.Foto = File.ReadAllBytes(caminhoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar a foto da indústria.", ex);
            }
        }
    }    
}
