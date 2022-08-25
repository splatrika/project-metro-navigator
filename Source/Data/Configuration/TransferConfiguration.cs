using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public class TransferConfiguration : WayConfiguration<Transfer>
{
    protected override void Configure2(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("Transfer");
    }
}

