using System;
namespace Source.Entities;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Line Line { get; set; }
    public Position Position { get; set; }

    public Station(string name, Line line, Position position)
    {
        Name = name;
        Line = line;
        Position = position;
    }
}

