using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class StaticDuration : DurationFactor
{
    public float StaticSeconds { get; set; }
    public override float Seconds => StaticSeconds;

    public StaticDuration(float seconds = 0, int id = 0) : base(id)
    {
        StaticSeconds = seconds;
    }

    public override bool Equals(object? obj)
    {
        if (obj is StaticDuration duration)
        {
            return duration.StaticSeconds == StaticSeconds;
        }
        return base.Equals(obj);
    }
}

