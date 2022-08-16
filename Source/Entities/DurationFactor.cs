using System;
namespace Source.Entities;

public abstract class DurationFactor
{
    public int Id { get; set; }
    public abstract float Seconds { get; }
}

