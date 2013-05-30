using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract
{
    public interface IRepository<TEntity> 
    {
        IQueryable<TEntity> Entities { get; }
        TEntity GetEntityByID(object ID);
        void DeleteByID(object ID);

        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        IEnumerable<TEntity> Get(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "");
    }
}
