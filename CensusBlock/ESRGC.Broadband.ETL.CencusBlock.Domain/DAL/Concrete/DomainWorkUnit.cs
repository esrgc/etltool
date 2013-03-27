using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Concrete
{
    public class DomainWorkUnit : IUnitOfWork, IDisposable
    {
        DomainDataContext _context;
        IRepository<ServiceCensusBlock> _serviceCensusRepo;

        public DomainWorkUnit(DomainDataContext context) {
            _context = context;
        }

        public void Dispose() {
            _context.Dispose();
        }

        public IRepository<Model.ServiceCensusBlock> ServiceCensusRepository {
            get {
                return _serviceCensusRepo ?? (_serviceCensusRepo = new Repository<ServiceCensusBlock>(_context));
            }
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }
    }
}
