using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using RaceAnalysis.Models;
using RaceAnalysis.Service;
using RaceAnalysis.Service.Interfaces;
using System.Data.Entity;
using System.Web.Http;
using Unity.WebApi;

namespace RaceAnalysisAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<DbContext, RaceAnalysisDbContext>(new HierarchicalLifetimeManager());
         //   container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
         //   container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
         //   container.RegisterType<RaceAnalysis.Controllers.AccountController>(new InjectionConstructor());
            container.RegisterType<IIdentityMessageService, EmailService>();
            container.RegisterType<IRaceService, RaceService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}