using System;
namespace Splatrika.MetroNavigator.Source.Entities;

public class Transfer : MapElement
{
    public Station From { get; set; }
    public Station To { get; set; }

    public Transfer(int id, Station from, Station to, Map map) : base(map)
    {
        Id = id;
        From = from;
        To = to;
    }
}

