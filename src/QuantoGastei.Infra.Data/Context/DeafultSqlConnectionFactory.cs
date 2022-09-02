using Npgsql;
using QuantoGastei.Infra.Data.Interfaces;
using System.Data;

namespace QuantoGastei.Infra.Data.Context
{
    public class DeafultSqlConnectionFactory : IConnectionFactory
    {
        #region [Propriedades Privadas]
        private readonly ParametrosConexao _parametros;
        #endregion

        #region [Métodos Privados]
        private NpgsqlConnection ObterStringConexaoSqlServer()
            => new($"Server={_parametros.Servidor},{_parametros.Porta};User Id={_parametros.Usuario};Password={_parametros.Senha};{(!string.IsNullOrWhiteSpace(_parametros.NomeBanco) ? $"Database={_parametros.NomeBanco.ToLower()}" : "")}");
        #endregion

        #region [Construtor]
        public DeafultSqlConnectionFactory(ParametrosConexao parametrosConexao)
        {
            _parametros = parametrosConexao;
        }
        #endregion

        #region [Métodos Públicos]
        public IDbConnection Conexao() => ObterStringConexaoSqlServer();
        #endregion
    }
}
