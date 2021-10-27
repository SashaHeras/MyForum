using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();

        public Task<TEntity> AddAsync(TEntity entity);

        public Task<TEntity> UpdateAsync(TEntity entity);
    }
}
