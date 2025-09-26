using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConnectorAccess.Service.data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ConnectorDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(ConnectorDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
