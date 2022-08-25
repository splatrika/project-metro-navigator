using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class RailwayConfiguration : WayConfiguration<Railway>
{
    protected override void Configure2(EntityTypeBuilder<Railway> builder)
    {
        builder.ToTable("Railways");
    }
}

