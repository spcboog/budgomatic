using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budgomatic.Core.Domain;
using NHibernate;

namespace Budgomatic.Core.DataAccess
{
    public class Repository : IRepository
    {
        private ISessionFactory _sessionFactory;

        public Repository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<TEntity> Get<TEntity>() 
            where TEntity: Entity
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    return session.QueryOver<TEntity>().Where(x => x.Deleted == false).List();
                }
            }
        }

        public TEntity Find<TEntity>(Guid id)
            where TEntity : Entity
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    return session.QueryOver<TEntity>().Where(
                        x => (x.Deleted == false) &&
                            (x.Id == id)
                            ).SingleOrDefault();
                }
            }
        }

        public TEntity Save<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    
                    transaction.Commit();

                    return entity;
                }
            }
        }

        public void Delete(Entity entity)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    entity.Deleted = true;
                    session.SaveOrUpdate(entity);

                    transaction.Commit();
                }
            }
        }
    }
}
