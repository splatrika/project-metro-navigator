using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class LineAppearanceConfiguration : IEntityTypeConfiguration<LineAppearance>
{
    public void Configure(EntityTypeBuilder<LineAppearance> builder)
    {
        builder.OwnsOne(e => e.Color);
        builder.HasIndex(e => e.LineId);
    }
}

