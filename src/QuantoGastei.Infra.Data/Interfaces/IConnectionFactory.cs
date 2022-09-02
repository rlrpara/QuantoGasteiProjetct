using System.Data;

namespace QuantoGastei.Infra.Data.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection Conexao();
    }
}
