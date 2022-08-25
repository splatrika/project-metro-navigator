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
}

