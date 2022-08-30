using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        public BaseRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_dbContext);
        }

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

    }
}
