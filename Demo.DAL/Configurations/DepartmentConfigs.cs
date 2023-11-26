using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.DAL.Configurations
{
    internal class DepartmentConfigs : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
           builder.HasMany(D => D.Employees)
              .WithOne(E => E.Department)
              .HasForeignKey(E => E.DepartmentId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property(D=>D.Code).IsRequired(true);
            builder.Property(D=>D.Name).IsRequired(true)
                .HasMaxLength(50);
            builder.Property(D => D.DateOfCreation).IsRequired(true)
                .HasColumnType("DateTime");
        }
    }
}
