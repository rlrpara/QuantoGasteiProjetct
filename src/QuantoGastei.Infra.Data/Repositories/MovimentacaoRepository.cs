using QuantoGastei.Domain.Interfaces;

namespace QuantoGastei.Infra.Data.Repositories
{
    public class MovimentacaoRepository : BaseRepository, IMovimentacaoRepository
    {
        #region [Propriedades Privadas]
        private readonly IBaseRepository _baseRepository;
        #endregion

        #region [Construtor]
        public MovimentacaoRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion

        #region [Métodos Públicos]

        #endregion
    }
}
