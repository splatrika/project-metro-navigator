using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public class LineAppearance : EntityBase
{
    public int LineId { get; private set; }
    public Color Color { get; set; }

#pragma warning disable 8618
    private LineAppearance() { } //Required by EF
#pragma warning restore 8618

    public LineAppearance(int lineId, Color color)
    {
        LineId = lineId;
        Color = color;
    }


    public static LineAppearance GetDefault(int id)
    {
        return new(id, new());
    }


    public override bool Equals(object? obj)
    {
        if (obj is LineAppearance appearance)
        {
            return appearance.LineId == LineId &&
                appearance.Color.Equals(Color);
        }
        return base.Equals(obj);
    }
}

