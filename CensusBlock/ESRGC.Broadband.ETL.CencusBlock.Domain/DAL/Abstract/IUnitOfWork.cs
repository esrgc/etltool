﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<ServiceCensusBlock> ServiceCensusRepository { get; }

        void SaveChanges();
    }
}