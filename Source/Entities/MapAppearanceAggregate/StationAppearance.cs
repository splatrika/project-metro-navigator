using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public class StationAppearance : EntityBase
{
    public int StationId { get; set; }
    public Position Position { get; set; }
    public Caption Caption { get; set; }

#pragma warning disable 8618
    private StationAppearance() { } //Required by EF
#pragma warning restore 8618

    public StationAppearance(int stationId, Position position, Caption caption)
    {
        StationId = stationId;
        Position = position;
        Caption = caption;
    }
}

