using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace Silverbear.Enterprise.Audit
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Edit the base address of AuditService by replacing the "AuditService" string below
            RouteTable.Routes.Add(new ServiceRoute("Audit", new WebServiceHostFactory(), typeof(AuditService)));
        }
    }
}