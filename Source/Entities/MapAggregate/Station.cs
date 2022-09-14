using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Station : EntityBase
{
    public string Name { get; set; }
    public Line Line { get; private set; }

#pragma warning disable 8618
    private Station() { } //Required by EF
#pragma warning restore 8618

    public Station(string name, Line line, int id = 0) : base(id)
    {
        Name = name;
        Line = line;
    }


    public override string ToString()
    {
        return $"Station {Name}";
    }
}

