using Microsoft.EntityFrameworkCore;
using System;

namespace Employee.Models
{
    public class EmpDepartmentDbContext : DbContext
    {
        public EmpDepartmentDbContext(DbContextOptions<EmpDepartmentDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Employees> Employee { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Employees>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).ValueGeneratedOnAdd();

                e.HasOne(a => a.DepartmentFK).WithMany(r => r.Employees).HasForeignKey(a => a.DepartmentIDFK);
            });

            modelBuilder.Entity<Department>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
