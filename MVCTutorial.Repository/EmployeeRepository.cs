using MVCTutorial.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTutorial.Repository
{
    public class EmployeeRepository:BaseRepository<Employee>
    {
        public EmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    }
}
