using System;
using System.Threading.Tasks;
using Management.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Management.Server.Database.Repositories
{
    public abstract class AbstractRepository<T>
    where T : AbstractModel
    {
        protected readonly DbSet<T> DbModel;

        protected readonly PostgreSqlContext Context;

        protected readonly ILogger<T> Logger;


        protected AbstractRepository(PostgreSqlContext context, ILoggerFactory loggerFactory)
        {
            Context = context;
            DbModel = context.Set<T>();
            Logger = loggerFactory.CreateLogger<T>();
        }

        public async Task<T> FindOne(int id)
        {
            var model = await DbModel.FindAsync(id);
            return model;
        }

        public void Delete(T entity)
        {
            DbModel.Remove(entity);
        }


        protected async Task<T> CreateModelAsync(T model)
        {
            await Context.AddAsync(model);
            var result = await Context.SaveChangesAsync();
            if (result == 0)
            {
                throw new Exception("Db error. Not Create any model");
            }

            return model;
        }


        public Task<int> UpdateModelAsync(T model)
        {
            DbModel.Update(model);
            return Context.SaveChangesAsync();
        }


        public async Task DeleteModel(T model)
        {
            Delete(model);
            var result = await Context.SaveChangesAsync();
            if (result == 0)
            {
                throw new Exception("Db error. Not deleted");
            }
        }
    }
}