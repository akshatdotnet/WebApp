using MVCTutorial.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Business.Interface
{
   public interface IDepartmentBusiness
    {
       List<DepartmentDomainModel> GetAllDepartment();
    }
}
