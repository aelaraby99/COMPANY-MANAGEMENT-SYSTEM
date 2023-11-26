using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DAL.Configurations
{
    internal class EmployeeConfigs : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Name)
                   .IsRequired(true)
                   .HasMaxLength(50);

            builder.Property(e=>e.Address)
                .IsRequired(true);  
        }
    }
}
