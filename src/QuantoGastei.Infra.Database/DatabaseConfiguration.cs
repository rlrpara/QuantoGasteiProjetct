using Dapper;
using QuantoGastei.Domain.Entities;
using QuantoGastei.Infra.Data.Context;
using System.Data;
using System.Text;

namespace QuantoGastei.Infra.Database
{
    public static class DatabaseConfiguration
    {
        #region [Propriedades Privadas]
        #endregion

        #region Métodos Privados
        private static IDbConnection ObterConexao(bool removerNomeBanco = false) => ConnectionConfiguration.AbrirConexao(ObterParametrosConexao(removerNomeBanco));
        private static ParametrosConexao ObterParametrosConexao(bool removerNomeBanco) => new()
        {
            NomeBanco = removerNomeBanco ? "" : Environment.GetEnvironmentVariable("Database"),
            Servidor = Environment.GetEnvironmentVariable("DbServer"),
            Porta = Environment.GetEnvironmentVariable("DbPort"),
            Usuario = Environment.GetEnvironmentVariable("DbUser"),
            Senha = Environment.GetEnvironmentVariable("Password")
        };
        private static string ObterSqlCriarBanco(string nomeBanco) => new StringBuilder().AppendLine($"CREATE DATABASE {nomeBanco};").ToString();
        private static bool ExisteBanco(IDbConnection conexao, string nomeBanco)
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($"SELECT NAME");
            sqlPesquisa.AppendLine($"  FROM MASTER.DBO.SYSDATABASES");
            sqlPesquisa.AppendLine($"WHERE NAME = N'{nomeBanco}'");

            return conexao.Query<string>(sqlPesquisa.ToString()).ToList().Count > 0;
        }
        private static void Criar(IDbConnection conexao, string sqlCondicao) => conexao.Execute(sqlCondicao);
        private static bool ExisteDados<T>(IDbConnection conexao) where T : class => conexao.QueryFirstOrDefault<int>($"SELECT COUNT(*) AS Total FROM {GeradorDapper.ObterNomeTabela<T>()};") > 0;
        private static bool ServidorAtivo()
        {
            try
            {
                ObterParametrosConexao(false);
                using var conexao = ObterConexao();
                conexao.Open();
                return conexao.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
        private static void CriaBaseDados(IDbConnection conexao, string nomeBanco)
        {
            Criar(conexao, GeradorDapper.CriaTabela<Usuario>(nomeBanco));
            Criar(conexao, GeradorDapper.CriaTabela<Categoria>(nomeBanco));
            Criar(conexao, GeradorDapper.CriaTabela<Movimentacao>(nomeBanco));
        }
        private static void ExecutarScripts(IDbConnection conexao)
        {
            try
            {
                var pastaScripts = Path.Combine(Directory.GetCurrentDirectory(), "scripts");

                if (Directory.Exists(pastaScripts))
                {
                    foreach (var script in Directory.GetFiles(pastaScripts, "*.sql"))
                    {
                        string dados = new StreamReader(script).ReadToEnd();
                        foreach (var item in dados.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                            if (!string.IsNullOrWhiteSpace(item))
                                conexao.Query<string>(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Métodos Públicos
        public static void GerenciarBanco()
        {
            try
            {
                if (ServidorAtivo())
                {
                    using var conexaoSemNomeBanco = ObterConexao(true);
                    using var conexao = ObterConexao();

                    string nomeBanco = ObterParametrosConexao(false).NomeBanco??"";

                    if (!ExisteBanco(conexaoSemNomeBanco, nomeBanco))
                        Criar(conexaoSemNomeBanco, ObterSqlCriarBanco(nomeBanco));

                    //Criar tabelas
                    CriaBaseDados(conexao, nomeBanco);

                    //executar scripts da versao
                    ExecutarScripts(conexao);
                }
                else
                {
                    throw new Exception("Base de dados Offline\\Inexistente.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
