using System;
namespace Splatrika.MetroNavigator.Source.Entities;

public abstract class DurationFactor
{
    public int Id { get; set; }
    public abstract float Seconds { get; }
}

