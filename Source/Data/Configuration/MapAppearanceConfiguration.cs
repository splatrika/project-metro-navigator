using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class MapAppearanceConfiguration : IEntityTypeConfiguration<MapAppearance>
{
    public void Configure(EntityTypeBuilder<MapAppearance> builder)
    {
        builder.HasMany(e => e.Lines)
            .WithOne()
            .IsRequired();

        builder.HasMany(e => e.Stations)
            .WithOne()
            .IsRequired();

        builder.HasMany(e => e.Railways)
            .WithOne()
            .IsRequired();

        builder.Navigation(e => e.Lines)
            .HasField("_lines")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Stations)
            .HasField("_stations")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Railways)
            .HasField("_railways")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(e => e.MapId)
            .IsUnique();
    }
}

