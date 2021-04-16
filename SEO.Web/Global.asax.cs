using Microsoft.AspNet.WebFormsDependencyInjection.Unity;
using SEO.Service.Interfaces;
using SEO.Service.Services;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace SEO.Web
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			InjectDependencies();

			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private void InjectDependencies()
		{
			var container = this.AddUnity();

			container.RegisterType<ITextAnalysisService, AnalysisService>();
			container.RegisterType<IURLAnalysisService, AnalysisService>();
		}
	}
}