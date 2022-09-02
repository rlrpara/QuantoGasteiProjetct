using QuantoGastei.Domain.Entities;
using QuantoGastei.Domain.Interfaces;
using QuantoGastei.Infra.Data.Repositories;
using QuantoGastei.Service.Interfaces;

namespace QuantoGastei.Service.Services
{
    public class MovimentacaoService : BaseService, IMovimentacaoService
    {
        #region [Propriedades Privadas]
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        #endregion

        #region [Construtor]
        public MovimentacaoService(IBaseRepository baseRepository)
        {
            _movimentacaoRepository = new MovimentacaoRepository(baseRepository);
        }
        #endregion

        #region [Métodos Públicos]
        public IEnumerable<Movimentacao> ObterTodos(string sqlWhere)
        {
            return _movimentacaoRepository.Obtertodos(sqlWhere);
        }
        #endregion
    }
}
