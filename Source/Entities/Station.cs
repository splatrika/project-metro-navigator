using System;
namespace Splatrika.MetroNavigator.Source.Entities;

public class Station : MapElement
{
    public string Name { get; set; }
    public Line Line { get; set; }
    public Position Position { get; set; }

    public Station(string name, Line line, Position position, Map map) : base(map)
    {
        Name = name;
        Line = line;
        Position = position;
    }
}

