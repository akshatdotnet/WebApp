using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MVCTutorial.Domain
{
    public class EmployeeDomainModel
    {

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int ExtraValue { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
