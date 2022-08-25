using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Transfer : Way
{
#pragma warning disable 8618
    private Transfer() { } //Required by EF
#pragma warning restore 8618

    public Transfer(Station from, Station to, DurationFactor duration,
        int id = 0) : base(from, to, duration, id)
    { }
}

