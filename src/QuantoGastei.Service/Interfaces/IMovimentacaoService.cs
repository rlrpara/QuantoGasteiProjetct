using QuantoGastei.Domain.Entities;

namespace QuantoGastei.Service.Interfaces
{
    public interface IMovimentacaoService : IBaseService
    {
        IEnumerable<Movimentacao> ObterTodos(string sqlWhere);
    }
}
