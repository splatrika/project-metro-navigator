using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public class RailwayAppearance : EntityBase
{
    public int RailwayId { get; private set; }
    public List<Position> Points { get; private set; }

#pragma warning disable 8618
    private RailwayAppearance() { } //Required by EF
#pragma warning restore 8618

    public RailwayAppearance(int railwayId, IEnumerable<Position>? points = null)
    {
        RailwayId = railwayId;
        Points = points == null ? new() : new(points);
    }
}

