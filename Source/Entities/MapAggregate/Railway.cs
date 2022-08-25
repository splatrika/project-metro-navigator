using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Railway : Way
{
#pragma warning disable 8618
    private Railway() { } //Required by EF
#pragma warning restore 8618

    public Railway(Station from, Station to, DurationFactor duration,
        int id = 0) : base(from, to, duration, id)
    { }
}

