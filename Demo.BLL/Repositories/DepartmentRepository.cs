using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
