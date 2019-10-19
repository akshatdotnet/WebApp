using MVCTutorial.Business.Interface;
using MVCTutorial.Domain;
using MVCTutorial.Repository;
using MVCTutorial.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Business
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly EmployeeRepository empRepository;
        private readonly DepartmentRepository deptRepository;
  

        public EmployeeBusiness(IUnitOfWork _unitOfWork)
        {

            unitOfWork = _unitOfWork;
            empRepository = new EmployeeRepository(unitOfWork);
            deptRepository = new DepartmentRepository(unitOfWork);
        }
      

        //----------------Used in Test controller--------------------------------

        #region

        public int TotalEmployeeRecord()
        {
            int count = empRepository.GetAll(x => x.IsDeleted == false).Count();

            return count;
        }
        public List<EmployeeDomainModel> GetEmployeeRecords(int pageNo, int pageSize)
        {
            List<EmployeeDomainModel> List = new List<EmployeeDomainModel>();

            List = empRepository.GetPagedRecords(y => y.IsDeleted == false, x => x.Name, pageNo, pageSize)

                                .Select(x => new EmployeeDomainModel
                                {
                                    Name = x.Name,
                                    EmployeeId = x.EmployeeId,
                                    DepartmentName = x.Department.DepartmentName,
                                    Address = x.Address,
                                }).ToList() ?? new List<EmployeeDomainModel>();


            return List;
        }
        public EmployeeDomainModel GetEmployeeById(int Id)
        {
            EmployeeDomainModel empDomainModel = new EmployeeDomainModel();
            Employee emp = empRepository.SingleOrDefault(x => x.EmployeeId == Id && x.IsDeleted == false);

            if (emp != null)
            {
                empDomainModel.EmployeeId = emp.EmployeeId;
                empDomainModel.DepartmentId = emp.DepartmentId;
                empDomainModel.Name = emp.Name;
                empDomainModel.Address = emp.Address;
            }

            return empDomainModel;
        }
        public EmployeeDomainModel AddEditEmployee(EmployeeDomainModel domainModel)
        {

            if (domainModel.EmployeeId > 0)
            {
                //update
                Employee employee = empRepository.SingleOrDefault(x => x.EmployeeId == domainModel.EmployeeId && x.IsDeleted == false);

                employee.DepartmentId = domainModel.DepartmentId;
                employee.Name = domainModel.Name;
                employee.Address = domainModel.Address;
                empRepository.Update(employee);


            }
            else
            {
                //Insert
                Employee employee = new Employee();
                employee.Address = domainModel.Address;
                employee.Name = domainModel.Name;
                employee.DepartmentId = domainModel.DepartmentId;
                employee.IsDeleted = false;
                empRepository.Insert(employee);

                domainModel.EmployeeId = employee.EmployeeId;

            }

            return domainModel;
        }
        public bool DeleteEmployee(int employeeId)
        {
            bool result = false;
            Employee employee = empRepository.SingleOrDefault(x => x.EmployeeId == employeeId && x.IsDeleted == false);

            if (employee != null)
            {
                employee.IsDeleted = true;
                empRepository.Update(employee);
                result = true;

            }
            return result;
        }

        //datatable search
        public int TotalEmployeeCount(string searchText)
        {

            int count = empRepository.GetAll(x => x.IsDeleted == false && (x.Name.Contains(searchText) || x.Department.DepartmentName.Contains(searchText) || x.Address.Contains(searchText))).Count();

            return count;
        }
        public List<EmployeeDomainModel> SearchEmployee(string searchText, int pageNo, int pageSize)
        {
            List<EmployeeDomainModel> List = new List<EmployeeDomainModel>();

            List = empRepository.GetPagedRecords(x => x.IsDeleted == false && (x.Name.Contains(searchText) || x.Department.DepartmentName.Contains(searchText) || x.Address.Contains(searchText)), y => y.Name, pageNo, pageSize)

                                .Select(x => new EmployeeDomainModel
                                {
                                    Name = x.Name,
                                    EmployeeId = x.EmployeeId,
                                    DepartmentName = x.Department.DepartmentName,
                                    Address = x.Address,
                                }).ToList();


            return List;
        }


        //datatable external search
        public int TotalEmployeeCountByEmployeeName(string searchText)
        {

            int count = empRepository.GetAll(x => x.Name.Contains(searchText) && x.IsDeleted == false).Count();

            return count;
        }
        public List<EmployeeDomainModel> SearchEmployeeByEmployeeName(string searchText, int pageNo, int pageSize)
        {
            List<EmployeeDomainModel> List = new List<EmployeeDomainModel>();

            List = empRepository.GetPagedRecords(x => x.Name.Contains(searchText) && x.IsDeleted == false, y => y.Name, pageNo, pageSize)

                                .Select(x => new EmployeeDomainModel
                                {
                                    Name = x.Name,
                                    EmployeeId = x.EmployeeId,
                                    DepartmentName = x.Department.DepartmentName,
                                    Address = x.Address,
                                }).ToList();


            return List;
        }


 



        #endregion


        //---------------Used in Repo controller-----------------------

        #region

        public List<EmployeeDomainModel> GetAllEmployee()
        {
            List<EmployeeDomainModel> list = empRepository.GetAll().Select(m => new EmployeeDomainModel { Name = m.Name, DepartmentName = m.Department.DepartmentName, Address = m.Address }).ToList();

            return list;
        }
        public string AddUpdateEmployee(EmployeeDomainModel empModel)
        {

            string result = "";
            if (empModel.EmployeeId > 0)
            {

                Employee emp = empRepository.SingleOrDefault(x => x.EmployeeId == empModel.EmployeeId);

                if (emp != null)
                {
                    emp.Name = empModel.Name;
                    emp.DepartmentId = empModel.DepartmentId;
                    emp.Address = empModel.Address;

                    empRepository.Update(emp);

                    result = "updated";

                }
            }
            else
            {
                Employee emp = new Employee();

                emp.Name = empModel.Name;
                emp.DepartmentId = empModel.DepartmentId;
                emp.Address = empModel.Address;
                emp.IsDeleted = false;

                var record = empRepository.Insert(emp);

                result = "Inserted";
            }

            return result;
        }

        #endregion

    }
}
