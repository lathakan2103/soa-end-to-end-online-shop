using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Utils;

namespace Core.Common.Data
{
    /// <summary>
    /// defines a abstract class that should implement
    /// a type of entity and some DbContext derived class
    /// </summary>
    /// <typeparam name="TEntity">Entity</typeparam>
    /// <typeparam name="TContext">DbContext derived class</typeparam>
    public abstract class DataRepositoryBase<TEntity, TContext> : IDataRepository<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
        where TContext : DbContext, new()
    {
        #region Abstract methods

        protected abstract TEntity AddEntity(TContext entityContext, TEntity entity);

        protected abstract TEntity UpdateEntity(TContext entityContext, TEntity entity);

        protected abstract IEnumerable<TEntity> GetEntities(TContext entityContext);

        protected abstract TEntity GetEntity(TContext entityContext, int id);

        #endregion

        #region Public methods

        public TEntity Add(TEntity entity)
        {
            using (var entityContext = new TContext())
            {
                var addedEntity = AddEntity(entityContext, entity);
                entityContext.SaveChanges();
                return addedEntity;
            }
        }

        public void Remove(TEntity entity)
        {
            using (var entityContext = new TContext())
            {
                entityContext.Entry(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var entityContext = new TContext())
            {
                var entity = GetEntity(entityContext, id);
                entityContext.Entry(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var entityContext = new TContext())
            {
                var existingEntity = UpdateEntity(entityContext, entity);

                SimpleMapper.PropertyMap(entity, existingEntity);

                entityContext.SaveChanges();
                return existingEntity;
            }
        }

        public IEnumerable<TEntity> Get()
        {
            using (var entityContext = new TContext())
            {
                return (GetEntities(entityContext)).ToArray().ToList();
            }
        }

        public TEntity Get(int id)
        {
            using (var entityContext = new TContext())
            {
                return GetEntity(entityContext, id);
            }
        }

        #endregion
    }
}
