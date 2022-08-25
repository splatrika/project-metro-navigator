using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class MapConfiguration : IEntityTypeConfiguration<Map>
{
    public void Configure(EntityTypeBuilder<Map> builder)
    {
        builder.HasMany(e => e.Lines)
            .WithOne()
            .IsRequired();

        builder.HasMany(e => e.Stations)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(e => e.Railways)
            .WithOne()
            .IsRequired()
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(e => e.Transfers)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Navigation(e => e.Lines)
            .HasField("_lines")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Stations)
            .HasField("_stations")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(e => e.Railways)
            .HasField("_railways")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

