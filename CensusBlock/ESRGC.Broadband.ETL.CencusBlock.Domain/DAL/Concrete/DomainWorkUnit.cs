﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using System.Data.Entity.Infrastructure;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Concrete
{
    public class DomainWorkUnit : IUnitOfWork, IDisposable
    {
        DomainDataContext _context;
        IRepository<ServiceCensusBlock> _serviceCensusRepo;
        IRepository<Submission> _submissionRepo;
        IRepository<Ticket> _ticketRepo;

        public DomainWorkUnit(DomainDataContext context) {
            _context = context;
        }

        public void Dispose() {
            _context.Dispose();
        }
        public void RenewContext() {
            _context.Dispose();
            _context = new DomainDataContext();

            _serviceCensusRepo = null;
            _submissionRepo = null;
            _ticketRepo = null;
        }
        public IRepository<Model.ServiceCensusBlock> ServiceCensusRepository {
            get {
                return _serviceCensusRepo ?? (_serviceCensusRepo = new Repository<ServiceCensusBlock>(_context));
            }
        }
        public IRepository<Submission> SubmissionRepository {
            get { return _submissionRepo ?? (_submissionRepo = new Repository<Submission>(_context)); }
        }
        public IRepository<Ticket> TicketRepository {
            get { return _ticketRepo ?? (_ticketRepo = new Repository<Ticket>(_context)); }
        }
        public void SaveChanges() {
            //try {
            _context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException ex) {
            //    ex.Entries.Single().Reload();
            //    _context.SaveChanges();
            //}

        }


    }
}
