using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly AppDbContext dbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Add(T item)
            => await dbContext.Set<T>().AddAsync(item);
        public void Delete(T item)
            => dbContext.Set<T>().Remove(item);
        public async Task<T> Get(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await dbContext.Employees.Include(e => e.Department).AsNoTracking().ToListAsync();
            }
            return await dbContext.Set<T>().ToListAsync();
        }
        public void Update(T item)
            => dbContext.Set<T>().Update(item);
    }
}
