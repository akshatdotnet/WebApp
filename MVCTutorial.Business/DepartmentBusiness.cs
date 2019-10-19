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
    public class DepartmentBusiness : IDepartmentBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly DepartmentRepository deptRepository;

        public DepartmentBusiness(IUnitOfWork _unitOfWork)
        {

            unitOfWork = _unitOfWork;
            deptRepository = new DepartmentRepository(unitOfWork);
        }

        public List<DepartmentDomainModel> GetAllDepartment()
        {
            List<DepartmentDomainModel> departmentList = deptRepository.GetAll().Select(x => new DepartmentDomainModel { DepartmentId = x.DepartmentId, DepartmentName = x.DepartmentName }).ToList();

            return departmentList;
        }
    }
}
