using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public AppDbContext DbContext { get; }

        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<IQueryable<Employee>> GetEmployeesByName(string Name)
        {
            var result = await DbContext.Employees
            .Where(e => e.Name.ToLower().Contains(Name.ToLower()))
            .ToListAsync();

            return result.AsQueryable();
        }

        public void DetachEntity(Employee entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
