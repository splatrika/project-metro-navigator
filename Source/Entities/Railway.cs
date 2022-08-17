using System;
namespace Splatrika.MetroNavigator.Source.Entities;

public class Railway : MapElement
{
    public Station From { get; set; }
    public Station To { get; set; }
    public DurationFactor Duration { get; set; }
    public List<Position> Points { get; set; }

    public Railway(Station from, Station to, DurationFactor duration,
        List<Position> points, Map map) : base(map)
    {
        From = from;
        To = to;
        Duration = duration;
        Points = points;
    }
}

