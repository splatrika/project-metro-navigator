using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class StaticDurationConfiguration : IEntityTypeConfiguration<StaticDuration>
{
    public void Configure(EntityTypeBuilder<StaticDuration> builder)
    {
        builder.Property(e => e.StaticSeconds)
            .HasColumnName("Seconds");
    }
}

