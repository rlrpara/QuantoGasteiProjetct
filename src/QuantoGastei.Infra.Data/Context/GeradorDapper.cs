using QuantoGastei.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace QuantoGastei.Infra.Data.Context
{
    public static class GeradorDapper
    {
        #region [Propriedades Privadas]
        private static string? _nomeBanco;
        #endregion

        #region Métodos Privados
        public static string ObterChavePrimaria<T>() where T : class
        {
            string? chavePrimaria = string.Empty;

            List<string> campos = new();

            foreach (PropertyInfo item in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => (p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault() as ColumnAttribute)?.Order))
            {
                Nota? nota = item.GetCustomAttribute(typeof(Nota)) as Nota;

                if (nota is not null && (nota.PrimaryKey || item.GetCustomAttributes().FirstOrDefault() is KeyAttribute))
                    chavePrimaria = item.GetCustomAttribute<ColumnAttribute>()?.Name;
            }
            return chavePrimaria;
        }
        private static string TipoPropriedade(PropertyInfo item, bool textMax) => item.PropertyType.Name?.ToLower() switch
        {
            "int32" => "int default null",
            "int64" => "bigint default null",
            "double" => "decimal(18,2)",
            "single" => "float",
            "datetime" => "timestamp default current_timestamp",
            "boolean" => "boolean not null",
            "nullable`1" => ObtemParaTipoNulo(item.PropertyType.FullName, textMax),
            _ => $"{(textMax ? "text" : "character varying(255)")} null",
        };
        private static string ObtemParaTipoNulo(string fullName, bool textMax)
        {
            if (fullName.Contains("Int32"))
                return "int default null";
            else if (fullName.Contains("DateTime"))
                return "datetime default null";
            else
                return $"{(textMax ? "text" : "character varying(255)")} null";
        }
        #endregion

        #region Métodos Públicos
        public static string ObterNomeTabela<T>() where T : class
        {
            dynamic tableattr = typeof(T).GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name.Equals("TableAttribute"));

            return (tableattr is not null ? tableattr.Name : "");
        }
        public static string RetornaSelect<T>(string nomeBanco = "") where T : class
        {
            string? chavePrimaria = string.Empty;
            List<string> campos = new();

            foreach (PropertyInfo item in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => (p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault() as ColumnAttribute)?.Order))
            {
                Nota? nota = item.GetCustomAttribute(typeof(Nota)) as Nota;

                if (nota is not null && nota.UseDatabase)
                {
                    var tipoCampo = item.PropertyType.Name;

                    if (nota.PrimaryKey || item.GetCustomAttributes().FirstOrDefault() is KeyAttribute)
                        chavePrimaria = item.GetCustomAttribute<ColumnAttribute>()?.Name;

                    if (nota.UseToGet && (item.GetCustomAttribute<ColumnAttribute>()?.Name != ""))
                    {
                        var coluna = item.GetCustomAttribute<ColumnAttribute>()?.Name;
                        campos.Add($"{coluna} AS {item.Name}");
                    }
                }
            }

            return string.Join($",{Environment.NewLine}       ", campos.ToArray());
        }
        public static string RetornaInsert<T>(T entidade) where T : class
        {
            List<string> campos = new();
            List<string> valores = new();

            foreach (PropertyInfo item in entidade.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => (p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault() as ColumnAttribute)?.Order))
            {
                var tipoCampo = item.PropertyType;
                Nota? nota = item.GetCustomAttribute(typeof(Nota)) as Nota;

                if (nota is not null && nota.UseToGet && !nota.PrimaryKey)
                {
                    var valor = item.GetValue(entidade);

                    if (valor != null)
                    {
                        campos.Add(item.GetCustomAttribute<ColumnAttribute>().Name);

                        if (tipoCampo.Name.ToLower().Contains("string"))
                            valores.Add($"'{valor.ToString().Replace("'", "`")}'");

                        else if (tipoCampo.Name.ToLower().Contains("datetime"))
                            valores.Add($"'{Convert.ToDateTime(valor):yyyy-MM-dd HH:mm:ss}'");

                        else if (tipoCampo.Name.ToLower().Contains("nullable`1"))
                            if (tipoCampo.ToString().Contains("datetime"))
                                valores.Add($"'{Convert.ToDateTime(valor):yyyy-MM-dd HH:mm:ss}'");

                            else if (tipoCampo.ToString().Contains("int32"))
                                valores.Add($"{valor}");

                            else
                                valores.Add($"'{valor}'");

                        else
                            valores.Add($"{valor}");
                    }
                }
            }

            var sqlInsert = new StringBuilder();

            sqlInsert.AppendLine($"INSERT INTO {ObterNomeTabela<T>()} ({string.Join(", ", campos.ToArray())})");
            sqlInsert.AppendLine($"            VALUES ({string.Join(", ", valores.ToArray())});");

            return sqlInsert.ToString();
        }
        public static string RetornaUpdate<T>(int id, T entidade) where T : class
        {
            string? campoChave = string.Empty;
            List<string> condicao = new();

            foreach (PropertyInfo item in entidade.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).OrderBy(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault())?.Order))
            {
                Nota? nota = item.GetCustomAttribute(typeof(Nota)) as Nota;

                if (nota is not null && nota.UseToGet && !nota.PrimaryKey)
                {
                    var valor = item.GetValue(entidade);
                    string? campo = item.GetCustomAttribute<ColumnAttribute>()?.Name;
                    var tipoCampo = item.PropertyType.Name.ToLower();

                    if (nota.PrimaryKey || item.GetCustomAttributes().FirstOrDefault() is KeyAttribute)
                        campoChave = item.GetCustomAttribute<ColumnAttribute>()?.Name;

                    if (valor is not null)
                    {
                        if (tipoCampo.Contains("string"))
                            condicao.Add($"{campo} = '{valor}'");

                        else if (tipoCampo.Contains("datetime"))
                            condicao.Add($"{campo} = '{Convert.ToDateTime(valor):yyyy-MM-dd HH:mm:ss}'");

                        else if (tipoCampo.Contains("nullable`1"))
                            if (tipoCampo.ToString().Contains("datetime"))
                                condicao.Add($"{campo} = '{Convert.ToDateTime(valor):yyyy-MM-dd HH:mm:ss}'");

                            else if (tipoCampo.ToString().Contains("int32"))
                                condicao.Add($"{campo} = {valor}");

                            else
                                condicao.Add($"{campo} = '{valor}'");
                        else
                            condicao.Add($"{campo} = {valor}");
                    }
                }
                else if (nota is not null && nota.PrimaryKey)
                {
                    campoChave = item.GetCustomAttribute<ColumnAttribute>()?.Name;
                }
            }

            var sqlAtualizar = new StringBuilder();

            sqlAtualizar.AppendLine($"UPDATE {ObterNomeTabela<T>()}");
            sqlAtualizar.AppendLine($"   SET {(string.Join($",{Environment.NewLine}       ", condicao.ToArray()))}");
            sqlAtualizar.AppendLine($" WHERE {campoChave} = {id}");

            return sqlAtualizar.ToString();
        }
        public static string RetornaDelete<T>(int id) where T : class
        {
            string? campoChave = string.Empty;

            foreach (PropertyInfo item in typeof(T).GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).OrderBy(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault())?.Order))
            {
                Nota notaBase = (Nota)item.GetCustomAttribute(typeof(Nota));

                if (notaBase is not null && notaBase.UseToGet && notaBase.PrimaryKey)
                    campoChave = item.GetCustomAttribute<ColumnAttribute>()?.Name;
            }

            var sqlDelete = new StringBuilder();

            sqlDelete.AppendLine($"USE {_nomeBanco};");
            sqlDelete.AppendLine($"DELETE FROM {ObterNomeTabela<T>()}");
            sqlDelete.AppendLine($" WHERE {campoChave} = {id}");

            return sqlDelete.ToString();
        }
        public static string ObterSqlTabela<T>(string nomeBanco) where T : class
        {
            string? chavePrimaria = string.Empty;
            StringBuilder indices = new();
            List<string> campos = new();
            StringBuilder sqlConstraint = new();
            _nomeBanco = nomeBanco;

            foreach (PropertyInfo item in typeof(T).GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
            {
                Nota? nota = item.GetCustomAttribute(typeof(Nota)) as Nota;

                if (nota is not null && item.GetCustomAttribute<ColumnAttribute>() is not null && nota.UseToGet)
                {
                    string? nomeCampo = item.GetCustomAttribute<ColumnAttribute>()?.Name;

                    if (nota.UseDatabase)
                    {
                        if (nota.PrimaryKey || item.GetCustomAttributes().FirstOrDefault() is KeyAttribute)
                            chavePrimaria = $"{nomeCampo}";

                        else if (nomeCampo != "")
                            campos.Add($"{nomeCampo} {TipoPropriedade(item, nota.TextMax)}");
                    }

                    if (nota.UseIndex)
                        indices.AppendLine(nomeCampo);

                    if (!string.IsNullOrEmpty(nota.ForeignKey))
                    {
                        string tabelaChaveEstrangeira = $"{nota.ForeignKey}";
                        string campoChaveEstrangeira = $"{nomeCampo}";
                        string nomeChave = $"FK_{ObterNomeTabela<T>()}_{campoChaveEstrangeira}".ToUpper();

                        sqlConstraint.AppendLine($"GO");
                        sqlConstraint.AppendLine($"ALTER TABLE {ObterNomeTabela<T>()}");
                        sqlConstraint.AppendLine($"ADD CONSTRAINT {nomeChave} FOREIGN KEY ({campoChaveEstrangeira})");
                        sqlConstraint.AppendLine($"REFERENCES {_nomeBanco}.{tabelaChaveEstrangeira} (ID) ON DELETE NO ACTION ON UPDATE NO ACTION;{Environment.NewLine}");
                    }
                }
            }

            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($"CREATE TABLE IF NOT EXISTS {ObterNomeTabela<T>()} (");
            sqlPesquisa.AppendLine($"  {chavePrimaria} SERIAL PRIMARY KEY,");
            sqlPesquisa.AppendLine($"  {string.Join($",{Environment.NewLine}   ", campos.ToArray())}");
            sqlPesquisa.AppendLine($")");

            if (!string.IsNullOrEmpty(sqlConstraint.ToString()))
                sqlPesquisa.AppendLine(sqlConstraint.ToString());

            foreach (var item in indices.ToString().Split("\r\n"))
            {
                sqlPesquisa.AppendLine($"GO");
                sqlPesquisa.AppendLine($"CREATE INDEX IF NOT EXISTS {item}_INDEX ON {ObterNomeTabela<T>} ({item});");
            }

            return sqlPesquisa.ToString();
        }
        public static string GeralSqlSelecaoControles<T>(string sqlWhere, string nomeBanco) where T : class
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($"SELECT {RetornaSelect<T>(nomeBanco)}");
            sqlPesquisa.AppendLine($"  FROM {ObterNomeTabela<T>()}");
            sqlPesquisa.AppendLine($"{(sqlWhere.Trim() == string.Empty ? string.Empty : $"WHERE {sqlWhere}")}");

            return sqlPesquisa.ToString();
        }
        #endregion
    }
}
}
