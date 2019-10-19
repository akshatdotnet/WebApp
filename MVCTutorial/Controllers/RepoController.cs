using MVCTutorial.Business.Interface;
using MVCTutorial.Domain;
using MVCTutorial.Models;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MVCTutorial.Controllers
{

    public class RepoController : Controller
    {

        IEmployeeBusiness empBusiness;

        public RepoController(IEmployeeBusiness _empBusiness)
        {
            empBusiness = _empBusiness;
        }

        // GET: Repo
        public ActionResult Index()
        {
            string result = AddEditEmployee();

            List<EmployeeDomainModel> listDomain = empBusiness.GetAllEmployee();
            
            List<EmployeeViewModel> listemployeeVM = new List<EmployeeViewModel>();

            AutoMapper.Mapper.Map(listDomain, listemployeeVM);

            ViewBag.EmployeeList = listemployeeVM;

            return View();
        }

        public string AddEditEmployee()
        {
            string result = "";
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.EmployeeId = 2014;
            vm.Name = "Laura";
            vm.DepartmentId = 3;
            vm.Address = "Australia";

            EmployeeDomainModel empDomainModel = new EmployeeDomainModel();

            AutoMapper.Mapper.Map(vm, empDomainModel);

            result = empBusiness.AddUpdateEmployee(empDomainModel);

            return result;

        }

    }


}