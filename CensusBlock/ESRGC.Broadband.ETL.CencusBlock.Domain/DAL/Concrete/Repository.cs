using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using ESRGC.Broadband.ETL.CencusBlock.Domain.DAL.Abstract;

namespace ESRGC.Broadband.ETL.CencusBlock.Domain.DAL.Concrete
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        internal IDbSet<TEntity> _dbSet;
        public Repository(){
            
        }

        public IQueryable<TEntity> Entities {
            get { throw new NotImplementedException(); }
        }

        public TEntity GetEntityByID(object ID) {
            throw new NotImplementedException();
        }

        public void DeleteByID(object ID) {
            throw new NotImplementedException();
        }

        public void InsertEntity(TEntity entity) {
            throw new NotImplementedException();
        }

        public void UpdateEntity(TEntity entity) {
            throw new NotImplementedException();
        }

        public void DeleteEntity(TEntity entity) {
            throw new NotImplementedException();
        }
    }
}
