using System;
namespace Source.Entities;

public class StaticDuration : DurationFactor
{
    public float StaticSeconds { get; set; }
    public override float Seconds => StaticSeconds;
}

