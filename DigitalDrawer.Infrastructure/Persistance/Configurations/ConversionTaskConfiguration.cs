using DigitalDrawer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Infrastructure.Persistance.Configurations
{
    internal class ConversionTaskConfiguration : IEntityTypeConfiguration<ConversionTask>
    {
        public void Configure(EntityTypeBuilder<ConversionTask> builder)
        {
            builder.HasMany(c => c.Files)
                .WithOne(f => f.ConversionTask)
                .HasForeignKey(f => f.ConversionTaskId);
        }
    }
}
