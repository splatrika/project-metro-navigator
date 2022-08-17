using System;
using System.ComponentModel.DataAnnotations;

namespace Splatrika.MetroNavigator.Source.Entities;

public class Map
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Line>? Lines { get; set; }
    public IEnumerable<Station>? Stations { get; set; }
    public IEnumerable<Railway>? Railways { get; set; }
    public IEnumerable<Transfer>? Transfers { get; set; }

    public Map(string name)
    {
        Name = name;
    }
}

