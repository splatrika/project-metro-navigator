using System;
namespace Source.Entities;

public class Line
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public Line(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}

