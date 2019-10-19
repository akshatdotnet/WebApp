using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MVCTutorial.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCTutorialEntitiesContainer _dbContext;

        public UnitOfWork()
        {
            _dbContext = new MVCTutorialEntitiesContainer();
        }

        public DbContext Db
        {
            get { return _dbContext; }
        }

        public void Dispose()
        {
        }
    }

}
