using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;
using Funq;
using ServiceStack.OrmLite;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using FYP_ChildAbuseGIS2013.ServiceInterface;

namespace FYP_ChildAbuseGIS2013
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>        
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost() : base("Hello Web Services", typeof(AnalysisDetailsService).Assembly) { }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "127.0.0.1", "5432", "postgres", "password", "ChildAbuseAnalysis");
            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connstring, PostgreSqlDialect.Provider));
            container.Register<ICacheClient>(new MemoryCacheClient());
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}