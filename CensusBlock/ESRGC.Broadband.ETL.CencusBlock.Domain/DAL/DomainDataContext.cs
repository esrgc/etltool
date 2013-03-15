using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL
{
    public class DomainDataContext: DbContext
    {
        public DomainDataContext() { 
        
        }

        /*public dbsets go here*/

        /// <summary>
        /// On creating event for initialization
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }
}
