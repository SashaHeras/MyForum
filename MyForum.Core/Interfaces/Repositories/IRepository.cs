using System.Linq;
using System.Threading.Tasks;
using MyForum.Core.Interfaces.Infrastructure;

namespace MyForum.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();

        public Task<TEntity> AddAsync(TEntity entity);

        public Task<TEntity> UpdateAsync(TEntity entity);
    }
}
