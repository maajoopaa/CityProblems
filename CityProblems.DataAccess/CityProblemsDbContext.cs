using CityProblems.DataAccess.Configurations;
using CityProblems.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityProblems.DataAccess
{
    public class CityProblemsDbContext : DbContext
    {
        public CityProblemsDbContext(DbContextOptions<CityProblemsDbContext> options)
            : base(options)
        { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<IssueEntity> Issues { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new IssueConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
