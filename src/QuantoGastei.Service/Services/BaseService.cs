using QuantoGastei.Domain.Interfaces;
using QuantoGastei.Infra.Data.Repositories;
using QuantoGastei.Service.Interfaces;

namespace QuantoGastei.Service.Services
{
    public class BaseService : IBaseService
    {
        #region [Propriedades Privadas]
        protected IBaseRepository _baseRepository;
        #endregion

        #region [Construtor]
        public BaseService()
        {
            _baseRepository = new BaseRepository();
        }
        #endregion

        #region [Métodos Públicos]
        public int Adicionar<T>(T entidade) where T : class
        {
            return _baseRepository.Adicionar(entidade);
        }

        public int Atualizar<T>(int id, T entidade) where T : class
        {
            return _baseRepository.Atualizar(id, entidade);
        }

        public T BuscarPorId<T>(int id) where T : class
        {
            return _baseRepository.BuscarPorId<T>(id);
        }

        public T BuscarPorQuery<T>(string query)
        {
            return _baseRepository.BuscarPorQuery<T>(query);
        }

        public T BuscarPorQueryGerador<T>(string sqlWhere = null) where T : class
        {
            return _baseRepository.BuscarPorQueryGerador<T>(sqlWhere); throw new NotImplementedException();
        }

        public IEnumerable<T> BuscarTodosPorQuery<T>(string query = null) where T : class
        {
            return _baseRepository.BuscarTodosPorQuery<T>(query);
        }

        public IEnumerable<T> BuscarTodosPorQueryGerador<T>(string sqlWhere = null) where T : class
        {
            return _baseRepository.BuscarTodosPorQueryGerador<T>(sqlWhere);
        }

        public int Excluir<T>(int id) where T : class
        {
            return _baseRepository.Excluir<T>(id);
        }

        public int? ObterUltimoRegistro<T>() where T : class
        {
            return _baseRepository.ObterUltimoRegistro<T>();
        }

        public List<T> Query<T>(string where) where T : class
        {
            return _baseRepository.Query<T>(where);
        }

        public void Query(string sql)
        {
            _baseRepository.Query(sql);
        }
        #endregion
    }
}
