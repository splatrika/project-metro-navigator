using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public abstract class DurationFactor: EntityBase
{
    public abstract float Seconds { get; }

    public DurationFactor(int id = 0) : base(id)
    {
    }
}

