using DigitalDrawer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Infrastructure.Persistance.Configurations;

internal class FileConversionConfiguration : IEntityTypeConfiguration<ConversionFile>
{
    public void Configure(EntityTypeBuilder<ConversionFile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ConvertedFileName)
            .HasMaxLength(256);
        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(256);
        builder.HasOne(x => x.ConversionTask)
            .WithMany(c => c.Files)
            .HasForeignKey(x => x.ConversionTaskId);
    }
}
