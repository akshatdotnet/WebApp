using MVCTutorial.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Business.Interface
{
    public interface IEmployeeBusiness
    {

        //----Below methods called in TEST Controller---

        //CRUD operation 

        int TotalEmployeeRecord();
        List<EmployeeDomainModel> GetEmployeeRecords(int pageNo, int pageSize);
        EmployeeDomainModel GetEmployeeById(int Id);
        EmployeeDomainModel AddEditEmployee(EmployeeDomainModel model);
        bool DeleteEmployee(int employeeId);

        //Search by Ename, department and country
        int TotalEmployeeCount(string searchText);
        List<EmployeeDomainModel> SearchEmployee(string searchText, int pageNo, int pageSize);

        //datatable external search example (by employeeName)
        int TotalEmployeeCountByEmployeeName(string searchText);
        List<EmployeeDomainModel> SearchEmployeeByEmployeeName(string searchText, int pageNo, int pageSize);


 

        #region

        //Basic repository example used in Repo Controller
        List<EmployeeDomainModel> GetAllEmployee();
        string AddUpdateEmployee(EmployeeDomainModel emp);
        
        #endregion


    }
}
