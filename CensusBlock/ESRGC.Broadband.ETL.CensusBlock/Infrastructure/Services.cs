using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Abstract;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL.Concrete;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL;


namespace ESRGC.Broadband.ETL.CensusBlock.Infrastructure
{
    public class Services: NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>()
                .To<DomainWorkUnit>()
                .WithConstructorArgument("context", new DomainDataContext());
        }
    }
}
