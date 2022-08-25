using System;
using System.ComponentModel.DataAnnotations;

namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Way : EntityBase
{
    public Station From { get; private set; }
    public Station To { get; private set; }
    public DurationFactor Duration { get; private set; }

#pragma warning disable 8618
    protected Way() { } //Required by EF
#pragma warning restore 8618

    public Way(Station from, Station to, DurationFactor duration,
        int id = 0) : base(id)
    {
        From = from;
        To = to;
        Duration = duration;
    }
}

