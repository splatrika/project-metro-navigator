using System;
namespace Source.Entities;

public class Railway
{
    public int Id { get; set; }
    public Station From { get; set; }
    public Station To { get; set; }
    public DurationFactor Duration { get; set; }
    public List<Position> Points { get; set; }

    public Railway(Station from, Station to, DurationFactor duration, List<Position> points)
    {
        From = from;
        To = to;
        Duration = duration;
        Points = points;
    }
}

