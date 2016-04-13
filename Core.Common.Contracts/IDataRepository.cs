using System.Collections.Generic;

namespace Core.Common.Contracts
{
    /// <summary>
    /// marker interface
    /// will be used by MEF to discover and create repository
    /// the graph will be:
    /// 
    ///     IDataRepository -
    ///        IDataRepository<T> - 
    ///             DataRepositoryBase<T, U> 
    /// 
    /// </summary>
    public interface IDataRepository
    {

    }

    /// <summary>
    /// will be used by DataRepositoryBase abstract class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataRepository<T> : IDataRepository
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);

        void Remove(T entity);

        void Remove(int id);

        T Update(T entity);

        IEnumerable<T> Get();

        T Get(int id);
    }
}
