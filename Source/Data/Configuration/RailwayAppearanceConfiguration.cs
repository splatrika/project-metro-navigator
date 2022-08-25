using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Data.Converters;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class RailwayAppearanceConfiguration : IEntityTypeConfiguration<RailwayAppearance>
{
    public void Configure(EntityTypeBuilder<RailwayAppearance> builder)
    {
        builder.Property(e => e.Points)
            .HasConversion<PointsListConverter>(new ValueComparer<List<Position>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()))
            .IsRequired();

        builder.HasIndex(e => e.RailwayId);
    }
}

