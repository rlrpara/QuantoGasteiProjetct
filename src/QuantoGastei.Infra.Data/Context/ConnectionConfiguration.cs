using System.Data;

namespace QuantoGastei.Infra.Data.Context
{
    public class ConnectionConfiguration
    {
        #region [Métodos Privados]
        private static IDbConnection? Inicia(IDbConnection conexao)
        {
            try
            {
                if (conexao != null)
                {
                    if (conexao.State == ConnectionState.Open) conexao.Close();
                    if (conexao.State == ConnectionState.Closed) conexao.Open();
                }
                return conexao;
            }
            catch
            {
                return default;
            }
        }
        #endregion

        #region Métodos Públicos
        public static IDbConnection? AbrirConexao(ParametrosConexao parametrosConexao)
            => Inicia(new DeafultSqlConnectionFactory(parametrosConexao).Conexao());
        #endregion
    }
}
