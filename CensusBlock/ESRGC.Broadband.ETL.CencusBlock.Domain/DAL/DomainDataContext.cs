using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.DAL
{
    public class DomainDataContext: DbContext
    {
        public DomainDataContext():base("name=ETL") { 
        
        }

        /*public dbsets go here*/
        public IDbSet<ServiceCensusBlock> ServiceCensusBlocks { get; set; }
        public IDbSet<Submission> Submissions { get; set; }
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
