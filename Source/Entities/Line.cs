using System;
using System.ComponentModel.DataAnnotations;
namespace Splatrika.MetroNavigator.Source.Entities;

public class Line : MapElement
{
    public string Name { get; set; }
    public Color Color { get; set; }

    public Line(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}

