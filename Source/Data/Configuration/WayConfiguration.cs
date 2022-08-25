using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Data.Configuration;

public abstract class WayConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Way
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<int>("FromId")
            .IsRequired();

        builder.Property<int>("ToId")
            .IsRequired();

        builder.HasOne(e => e.From)
            .WithMany()
            .IsRequired();

        builder.HasOne(e => e.To)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        Configure2(builder);
    }

    protected abstract void Configure2(EntityTypeBuilder<T> builder);
}

