using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class DurationFactorConfiguration : IEntityTypeConfiguration<DurationFactor>
{
    public void Configure(EntityTypeBuilder<DurationFactor> builder)
    {
        builder.HasDiscriminator<string>("Type");
    }
}

