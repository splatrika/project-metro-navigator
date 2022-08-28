using System;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public class MapAppearance : EntityBase, IAggregateRoot
{
    public int MapId { get; private set; }
    public IReadOnlyCollection<LineAppearance> Lines
        => _lines.AsReadOnly();
    public IReadOnlyCollection<RailwayAppearance> Railways
        => _railways.AsReadOnly();
    public IReadOnlyCollection<StationAppearance> Stations
        => _stations.AsReadOnly();

    private List<LineAppearance> _lines;
    private List<RailwayAppearance> _railways;
    private List<StationAppearance> _stations;


#pragma warning disable 8618
    private MapAppearance() { } //Required by EF
#pragma warning restore 8618


    public MapAppearance(int mapId)
    {
        MapId = mapId;
        _lines = new();
        _railways = new();
        _stations = new();
    }


    public LineAppearance UpdateLine(int lineId, Color color)
    {
        var line = GetLine(lineId);
        line.Color = color;
        return line;
    }


    public RailwayAppearance UpdateRailway(int railwayId, List<Position> points)
    {
        var railway = GetRailway(railwayId);
        railway.Points.Clear();
        railway.Points.AddRange(points);
        return railway;
    }


    public StationAppearance UpdateStation(int stationId, Position position,
        Caption caption)
    {
        var station = GetStation(stationId);
        station.Position = position;
        station.Caption = caption;
        return station;
    }


    public LineAppearance GetLine(int id)
    {
        var line = _lines.SingleOrDefault(x => x.LineId == id);
        if (line == null)
        {
            line = new(id, new());
            _lines.Add(line);
        }
        return line;
    }


    public RailwayAppearance GetRailway(int id)
    {
        var railway = _railways.SingleOrDefault(x => x.RailwayId == id);
        if (railway == null)
        {
            railway = new(id);
            _railways.Add(railway);
        }
        return railway;
    }


    public StationAppearance GetStation(int id)
    {
        var station = _stations.SingleOrDefault(x => x.StationId == id);
        if (station == null)
        {
            station = new(id, new Position(), Caption.Default);
            _stations.Add(station);
        }
        return station;
    }


    public void CleanUpLine(int lineId)
    {
        var line = _lines.SingleOrDefault(x => x.LineId == lineId);
        if (line != null)
        {
            _lines.Remove(line);
        }
    }


    public void CleanUpRailway(int railwayId)
    {
        var railway = _railways.SingleOrDefault(x => x.RailwayId == railwayId);
        if (railway != null)
        {
            _railways.Remove(railway);
        }
    }


    public void CleanUpStation(int stationId)
    {
        var station = _stations.SingleOrDefault(x => x.StationId == stationId);
        if (station != null)
        {
            _stations.Remove(station);
        }
    }
}

