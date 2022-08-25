using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
namespace Splatrika.MetroNavigator.Tests.Unit.Entities;

public class MapAppearanceTests
{
    [Fact]
    public void GetDefault()
    {
        const int id = 2;
        var appearance = new MapAppearance(id);
        Assert.Equal(id, appearance.MapId);

        var line = appearance.GetLine(id);
        Assert.Equal(id, line.LineId);
        Assert.Equal(new Color(), line.Color);

        var station = appearance.GetStation(id);
        Assert.Equal(id, station.StationId);
        Assert.Equal(new Position(), station.Position);
        Assert.Equal(Caption.Default, station.Caption);

        var railway = appearance.GetRailway(id);
        Assert.Equal(id, railway.RailwayId);
        Assert.True(railway.Points.SequenceEqual(new List<Position>()));
    }


    [Fact]
    public void EditDefault()
    {
        const int id = 2;
        var color = Color.White;
        var position = new Position(20, -3.4f);
        var points = new List<Position>()
        {
            new(2, 3),
            new(4.5f, -3.44f)
        };
        var caption = new Caption(new Position(2, -4), TextAligin.Center);

        var appearance = new MapAppearance(id);

        var line = appearance.GetLine(id);
        line.Color = color;
        line = appearance.GetLine(id);
        Assert.Equal(id, line.LineId);
        Assert.Equal(color, line.Color);

        var station = appearance.GetStation(id);
        station.Position = position;
        station.Caption = caption;
        station = appearance.GetStation(id);
        Assert.Equal(id, station.StationId);
        Assert.Equal(position, station.Position);
        Assert.Equal(caption, station.Caption);

        var railway = appearance.GetRailway(id);
        railway.Points.AddRange(points);
        railway = appearance.GetRailway(id);
        Assert.Equal(id, railway.RailwayId);
        Assert.True(railway.Points.SequenceEqual(points));
    }


    [Fact]
    public void InitialSet()
    {
        const int id = 2;
        var color = Color.White;
        var position = new Position(20, -3.4f);
        var points = new List<Position>()
        {
            new(2, 3),
            new(4.5f, -3.44f)
        };
        var appearance = new MapAppearance(id);
        var caption = new Caption(new Position(2, -4), TextAligin.Center);

        appearance.UpdateLine(id, color);
        var line = appearance.GetLine(id);
        Assert.Equal(id, line.LineId);
        Assert.Equal(color, line.Color);

        appearance.UpdateStation(id, position, caption);
        var station = appearance.GetStation(id);
        Assert.Equal(id, station.StationId);
        Assert.Equal(position, station.Position);
        Assert.Equal(caption, station.Caption);

        appearance.UpdateRailway(id, points);
        var railway = appearance.GetRailway(id);
        Assert.Equal(id, railway.RailwayId);
        Assert.True(railway.Points.SequenceEqual(points));

    }


    [Fact]
    public void Update()
    {
        const int id = 2;
        var color = Color.White;
        var position = new Position(20, -3.4f);
        var points = new List<Position>()
        {
            new(2, 3),
            new(4.5f, -3.44f)
        };
        var appearance = new MapAppearance(id);
        var caption = new Caption(new Position(2, -4), TextAligin.Center);

        appearance.UpdateLine(id, new());
        var line = appearance.GetLine(id);
        appearance.UpdateLine(id, color);
        Assert.Equal(id, line.LineId);
        Assert.Equal(color, line.Color);
        Assert.Contains(line, appearance.Lines);

        appearance.UpdateStation(id, new(), Caption.Default);
        var station = appearance.GetStation(id);
        appearance.UpdateStation(id, position, caption);
        Assert.Equal(id, station.StationId);
        Assert.Equal(position, station.Position);
        Assert.Equal(caption, station.Caption);
        Assert.Contains(station, appearance.Stations);


        appearance.UpdateRailway(id, new());
        var railway = appearance.GetRailway(id);
        appearance.UpdateRailway(id, points);
        Assert.Equal(id, railway.RailwayId);
        Assert.True(railway.Points.SequenceEqual(points));
        Assert.Contains(railway, appearance.Railways);
    }


    [Fact]
    public void CleanUp()
    {
        const int id = 2;
        var points = new List<Position>()
        {
            new(2, 3),
            new(4.5f, -3.44f)
        };
        var appearance = new MapAppearance(id);
        var caption = new Caption(new Position(2, -4), TextAligin.Center);

        var line = appearance.UpdateLine(id, new(1, 2, 3, 10));
        appearance.CleanUpLine(id);
        Assert.DoesNotContain(line, appearance.Lines);

        var station = appearance.UpdateStation(id, new(1, 2), caption);
        appearance.CleanUpStation(id);
        Assert.DoesNotContain(station, appearance.Stations);

        var railway = appearance.UpdateRailway(id, points);
        appearance.CleanUpRailway(id);
        Assert.DoesNotContain(railway, appearance.Railways);
    }
}

