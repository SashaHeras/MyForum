using MyForum.Controllers.Data;
using MyForum.Controllers.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly MyForumContext repositoryContext;

        public Repository(MyForumContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return repositoryContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($" We can`t get entites!!!\n {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                await repositoryContext.AddAsync(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null!!!");
            }

            try
            {
                repositoryContext.Update(entity);
                await repositoryContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
