using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WebService.Request;
using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints;
using Funq;

namespace WebService
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Define your ServiceStack web service response (i.e. Response DTO).
        /// </summary>
        public class HelloResponse
        {
            public string Result { get; set; }
        }

        /// <summary>
        /// Create your ServiceStack web service implementation.
        /// </summary>
        public class HelloService : IService
        {
            public object Any(File request)
            {
                //Looks strange when the name is null so we replace with a generic name.
                //var name = request.id ?? "John Doe";
                return new HelloResponse { Result = "Hello, " + 1 };
            }

            public object testing(File request)
            {
                return new HelloResponse { Result = "Hallo, " + 2 };
            }
        }

        /// <summary>
        /// Create your ServiceStack web service application with a singleton AppHost.
        /// </summary>        
        public class AnalysisHost : AppHostBase
        {
            /// <summary>
            /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
            /// </summary>
            public AnalysisHost() : base("Hello Web Services", typeof(HelloService).Assembly) { }

            /// <summary>
            /// Configure the container with the necessary routes for your ServiceStack application.
            /// </summary>
            /// <param name="container">The built-in IoC used with ServiceStack.</param>
            public override void Configure(Container container)
            {
                /*Register user-defined REST-ful urls. You can access the service at the url similar to the following.
                //http://localhost/ServiceStack.Hello/servicestack/hello or http://localhost/ServiceStack.Hello/servicestack/hello/John%20Doe
                //You can change /servicestack/ to a custom path in the web.config.
                Routes
                  .Add<Hello>("/hello")
                  .Add<Hello>("/hello/{Name}");*/
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            (new AnalysisHost()).Init();
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