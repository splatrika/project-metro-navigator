using System;
namespace Splatrika.MetroNavigator.Source.Entities.MapAggregate;

public class Map : EntityBase
{
    public string Name { get; set; }
    public IReadOnlyCollection<Line> Lines => _lines.AsReadOnly();
    public IReadOnlyCollection<Station> Stations => _stations.AsReadOnly();
    public IReadOnlyCollection<Railway> Railways => _railways.AsReadOnly();
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();

    private List<Line> _lines;
    private List<Station> _stations;
    private List<Railway> _railways;
    private List<Transfer> _transfers;

#pragma warning disable 8618
    private Map() { } //Required by EF
#pragma warning restore 8618


    public Map(string name, int id = 0) : base(id)
    {
        Name = name;
        _lines = new();
        _stations = new();
        _railways = new();
        _transfers = new();
    }


    public Line CreateLine(string name, int id = 0)
    {
        if (_lines == null) _lines = new();
        var line = new Line(name, id);
        _lines.Add(line);
        return line;
    }


    public Station CreateStation(int lineId, string name, int id = 0)
    {
        Require(_lines, "Lines");
        if (_stations == null) _stations = new();
        var line = _lines.SingleOrDefault(x => x.Id == lineId);
        if (line == null)
        {
            throw new InvalidOperationException(
                $"There isn't line with given id (Or it isn't include)");
        }
        var station = new Station(name, line, id);
        _stations.Add(station);
        return station;
    }


    public Railway CreateRailway(int lineId, int fromStationId, int toStationId,
        DurationFactor duration, int id = 0)
    {
        return CreateWay(_railways, fromStationId, toStationId, duration,
            onValidate: (from, to, duration) =>
            {
                Require(from.Line, "Lines");
                Require(to.Line, "Lines");
                if (from.Line != to.Line)
                {
                    throw new InvalidOperationException(
                        "Unable to create railway between stations of " +
                        "different lines");
                }
                if (from.Line.Id != lineId)
                {
                    throw new InvalidOperationException(
                        "Specified line hasn't got given station(s)");
                }
            },
            create: (from, to, duration) => new(from, to, duration, id));
    }


    public Transfer CreateTransfer(int fromStationId, int toStationId,
        DurationFactor duration, int id = 0)
    {
        return CreateWay(_transfers, fromStationId, toStationId, duration,
            onValidate: (from, to, duration) => { },
            create: (from, to, duration) => new(from, to, duration, id));
    }


    public void RemoveLine(int id)
    {
        Require(_lines, "Lines");
        Require(_stations, "Stations");
        var line = _lines.Single(x => x.Id == id);
        var relatedStations = _stations.Where(x => x.Line.Id == id)
            .ToArray();
        foreach (var station in relatedStations) RemoveStation(station.Id);
        _lines.Remove(line);
    }


    public void RemoveStation(int id)
    {
        Require(_stations, "Stations");
        var station = _stations.Single(x => x.Id == id);
        RemoveLinkedWays(_railways, station);
        RemoveLinkedWays(_transfers, station);
        _stations.Remove(station);
    }


    public void RemoveRailway(int id)
    {
        RemoveWay(_railways, id);
    }


    public void RemoveTransfer(int id)
    {
        RemoveWay(_transfers, id);
    }


    private void Require(object value, string caption)
    {
        if (value == null)
        {
            throw new InvalidOperationException($"{caption} should be included");
        }
    }


    private T CreateWay<T>(List<T> target, int fromStationId, int toStationId,
        DurationFactor duration, Action<Station, Station, DurationFactor> onValidate,
        Func<Station, Station, DurationFactor, T> create) where T : Way
    {
        if (fromStationId == toStationId)
        {
            throw new InvalidOperationException(
                "Unable to create way between the same station");
        }
        Require(_lines, "Lines");
        Require(_stations, "Stations");
        var from = _stations.SingleOrDefault(x => x.Id == fromStationId);
        var to = _stations.SingleOrDefault(x => x.Id == toStationId);
        if (from == null || to == null)
        {
            throw new InvalidOperationException(
                $"There isn't station with given id (Or it isn't include)");
        }
        onValidate(from, to, duration);
        var way = create(from, to, duration);
        target.Add(way);
        return way;
    }


    private void RemoveWay<T>(List<T> from, int id) where T : Way
    {
        Require(from, $"{typeof(T).Name}s");
        var way = from.SingleOrDefault(x => x.Id == id);
        if (way == null)
        {
            throw new InvalidOperationException(
                $"There isn't {nameof(T).ToLower()} with given id (Or it " +
                $"isn't included)");
        }
        from.Remove(way);
    }


    private void RemoveLinkedWays<T>(List<T> from, Station station) where T : Way
    {
        Require(from, $"{typeof(T).Name}s");
        var ways = from.Where(x => x.From == station || x.To == station).ToArray();
        foreach (var way in ways) from.Remove(way);
    }
}

