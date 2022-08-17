using System;
using System.ComponentModel.DataAnnotations;
namespace Splatrika.MetroNavigator.Source.Entities;

public class MapElement
{
    public int Id { get; set; }
    public Map Map { get; set; }

    public MapElement(Map map)
    {
        Map = map;
    }
}

