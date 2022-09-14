using QuantoGastei.Domain.Entities;

namespace QuantoGastei.Domain.Interfaces
{
    public interface IMovimentacaoRepository : IBaseRepository
    {
        IEnumerable<Movimentacao> Obtertodos(string sqlWhere);
    }
}
