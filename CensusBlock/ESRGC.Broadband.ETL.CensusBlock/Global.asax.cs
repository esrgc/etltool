﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ESRGC.Broadband.ETL.CensusBlock.Infrastructure;
using System.Data.Entity;
using ESRGC.Broadband.ETL.CensusBlock.Domain.DAL;
using ESRGC.Broadband.ETL.CensusBlock.Migrations;

namespace ESRGC.Broadband.ETL.CensusBlock
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //set ninject controller factory
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DomainDataContext, Configuration>());
        }
    }
}