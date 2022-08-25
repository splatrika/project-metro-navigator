using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class StaticDuration : DurationFactor
{
    public float StaticSeconds { get; set; }
    public override float Seconds => StaticSeconds;
}

