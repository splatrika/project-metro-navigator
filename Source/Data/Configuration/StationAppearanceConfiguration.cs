using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class StationAppearanceConfiguration : IEntityTypeConfiguration<StationAppearance>
{
    public void Configure(EntityTypeBuilder<StationAppearance> builder)
    {
        builder.OwnsOne(e => e.Position);

        builder.OwnsOne(e => e.Caption)
            .OwnsOne(e => e.Offset);

        builder.OwnsOne(e => e.Caption)
            .Property(e => e.TextAligin)
            .HasConversion<string>()
            .HasColumnType("nvarchar(10)");

        builder.HasIndex(e => e.StationId);
    }
}

