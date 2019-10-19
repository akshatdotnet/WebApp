using MVCTutorial.Business;
using MVCTutorial.Business.Interface;
using MVCTutorial.Repository.Infrastructure;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace MVCTutorial
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            //container.RegisterType<EmployeeBusiness>(new InjectionConstructor(0));
            container.RegisterType<IEmployeeBusiness, EmployeeBusiness>();
            container.RegisterType<IDepartmentBusiness, DepartmentBusiness>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

           
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}