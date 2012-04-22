using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.DataAccess
{
    public interface IRepository
    {
        IEnumerable<TEntity> Get<TEntity>() where TEntity : Entity;

        TEntity Find<TEntity>(Guid id) where TEntity : Entity;

        TEntity Save<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete(Entity entity);
    }
}
