using CityProblems.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityProblems.DataAccess.Configurations
{
    internal class IssueConfiguration : IEntityTypeConfiguration<IssueEntity>
    {
        public void Configure(EntityTypeBuilder<IssueEntity> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.CreatedBy)
                .WithMany(u => u.Issues)
                .HasForeignKey(i => i.CreatedById);

            builder.HasOne(i => i.Category)
                .WithMany(c => c.Issues)
                .HasForeignKey(i => i.CategoryId);
        }
    }
}
