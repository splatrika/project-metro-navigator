using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Line : EntityBase
{
    public string Name { get; set; }

#pragma warning disable 8618
    private Line() { } //Required by EF
#pragma warning restore 8618

    public Line(string name, int id = 0) : base(id)
    {
        Name = name;
    }


    public override string ToString()
    {
        return $"Line {Name}";
    }
}

