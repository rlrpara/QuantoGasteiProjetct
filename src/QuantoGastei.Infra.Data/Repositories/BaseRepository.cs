using Dapper;
using QuantoGastei.Domain.Interfaces;
using QuantoGastei.Infra.Data.Context;
using System.Data;
using System.Text;

namespace QuantoGastei.Infra.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        #region [Propriedades Privadas]
        private readonly CommandType _commandType;
        private readonly int? _timeOut = 800000000;
        #endregion

        #region [Métodos Privados]
        private static IDbConnection ObterConexao(bool removerNomeBanco = false) => ConnectionConfiguration.AbrirConexao(ObterParametrosConexao(removerNomeBanco));
        private static ParametrosConexao ObterParametrosConexao(bool removerNomeBanco) => new()
        {
            NomeBanco = removerNomeBanco ? "" : Environment.GetEnvironmentVariable("Database"),
            Servidor = Environment.GetEnvironmentVariable("DbServer"),
            Porta = Environment.GetEnvironmentVariable("DbPort"),
            Usuario = Environment.GetEnvironmentVariable("DbUser"),
            Senha = Environment.GetEnvironmentVariable("Password")
        };
        private static string ObterNomeTabela<T>()
        {
            return TableNameMapper(typeof(T));
        }
        private static string TableNameMapper(Type type)
        {
            dynamic tableattr = type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name.Equals("TableAttribute"));
            return (tableattr != null ? tableattr.Name : string.Empty);
        }
        #endregion

        #region [Construtor]
        public BaseRepository()
        {
        }
        #endregion

        #region [Métodos Públicos]
        public int Adicionar<T>(T entidade) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return conn.Execute(GeradorDapper.RetornaInsert(entidade), commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public int Atualizar<T>(int id, T entidade) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return conn.Execute(GeradorDapper.RetornaUpdate(id, entidade), commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public T BuscarPorId<T>(int id) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return conn.QueryFirstOrDefault<T>($"{GeradorDapper.RetornaSelect<T>(ObterParametrosConexao(false).NomeBanco)}", commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public T BuscarPorQuery<T>(string query)
        {
            try
            {
                using var conn = ObterConexao();
                return SqlMapper.QueryFirstOrDefault<T>(conn, query, commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public T BuscarPorQueryGerador<T>(string sqlWhere = null) where T : class
        {
            try
            {
                StringBuilder sqlPesquisa = new();

                sqlPesquisa.Append($"{GeradorDapper.RetornaSelect<T>(ObterParametrosConexao(false).NomeBanco)}");

                if (!string.IsNullOrEmpty(sqlWhere))
                    sqlPesquisa.Append($"AND {sqlWhere}");

                using var conn = ObterConexao();
                return conn.QueryFirstOrDefault<T>(sqlPesquisa.ToString(), commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public IEnumerable<T> BuscarTodosPorQuery<T>(string query = null) where T : class
        {
            try
            {
                var sqlPesquisa = new StringBuilder();

                sqlPesquisa.AppendLine(string.IsNullOrEmpty(query)
                    ? $"{GeradorDapper.RetornaSelect<T>(ObterParametrosConexao(false).NomeBanco)}"
                    : $"{query.Trim()}");

                using var conn = ObterConexao();
                return conn.Query<T>(sqlPesquisa.ToString(), commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public IEnumerable<T> BuscarTodosPorQueryGerador<T>(string sqlWhere = null) where T : class
        {
            try
            {
                var sqlPesquisa = new StringBuilder();

                sqlPesquisa.Append($"{GeradorDapper.RetornaSelect<T>(ObterParametrosConexao(false).NomeBanco)}");

                if (!string.IsNullOrEmpty(sqlWhere))
                    sqlPesquisa.Append($"AND {sqlWhere}");

                using var conn = ObterConexao();
                return conn.Query<T>(sqlPesquisa.ToString(), commandType: _commandType, commandTimeout: _timeOut).ToList();
            }
            catch
            {
                return (default);
            }
        }
        public int Excluir<T>(int id) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return conn.Execute($"{GeradorDapper.RetornaDelete<T>(id)}", commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public int? ObterUltimoRegistro<T>() where T : class
        {
            try
            {
                var sqlPesquisa = new StringBuilder();

                sqlPesquisa.AppendLine($"  SELECT TOP 1 {GeradorDapper.ObterChavePrimaria<T>()}");
                sqlPesquisa.AppendLine($"    FROM {ObterNomeTabela<T>()}");
                sqlPesquisa.AppendLine($"ORDER BY {GeradorDapper.ObterChavePrimaria<T>()} DESC");

                using var conn = ObterConexao();
                return conn.QueryFirstOrDefault<int?>(sqlPesquisa.ToString(), commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                return (default);
            }
        }
        public List<T> Query<T>(string where) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return conn.Query<T>(where, commandType: _commandType, commandTimeout: _timeOut).ToList();
            }
            catch
            {
                return (default);
            }
        }
        public void Query(string sql)
        {
            try
            {
                using var conn = ObterConexao();
                SqlMapper.Query(conn, sql, commandType: _commandType, commandTimeout: _timeOut);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
