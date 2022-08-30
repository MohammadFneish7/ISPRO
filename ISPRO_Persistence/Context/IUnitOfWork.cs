using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPRO.Persistence.Context
{
    public interface IUnitOfWork
    {
        BaseRepository<TEntity> Repository<TEntity>() where TEntity : class;

        void Commit();

    }
}
