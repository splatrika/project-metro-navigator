using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
namespace Splatrika.MetroNavigator.Tests.Unit.Entities;

public class MapTests
{
    [Fact]
    public void CreateLine()
    {
        const int id = 10;
        const string name = "London";
        var color = new Color { RedColor = 0.5f };
        var map = new Map("UK");
        var line = map.CreateLine(name, id);
        Assert.Equal(name, line.Name);
        Assert.Equal(id, line.Id);
        line = map.Lines.SingleOrDefault(
            x => x.Id == id && x.Name == name);
        Assert.NotNull(line);
    }


    [Fact]
    public void CreateStation()
    {
        const int lineId = 20;
        const int stationId = 78;
        const string stationName = "aaa";
        var position = new Position(2, -3);
        var map = new Map("-u-");
        var line = map.CreateLine("UwU", lineId);
        var station = map.CreateStation(lineId, stationName, stationId);
        Assert.Equal(stationName, station.Name);
        Assert.Equal(stationId, station.Id);
        station = map.Stations.SingleOrDefault(x =>
            x.Name == stationName &&
            x.Line == line &&
            x.Id == stationId);
        Assert.NotNull(station);
    }


    [Fact]
    public void CreateInvalidStation()
    {
        const int lineId = 20;
        const int invalidLineId = 223;
        var map = new Map("-u-");
        map.CreateLine("UwU", lineId);
        Assert.Throws<InvalidOperationException>(
            () => map.CreateStation(invalidLineId, "o-o", new()));
    }


    [Fact]
    public void CreateRailway()
    {
        const int lineId = 20;
        const int stationId1 = 78;
        const int stationId2 = 79;
        const int railwayId = 3;
        var testPoints = new List<Position>()
        {
            new(-2, 34),
            new(0, 2.3f),
            new(4.3f, -23)
        };
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line = map.CreateLine("UwU", lineId);
        var station1 = map.CreateStation(lineId, "u",  stationId1);
        var station2 = map.CreateStation(lineId, "o", stationId2);

        var railway = map.CreateRailway(lineId, stationId1, stationId2, duration,
            railwayId);
        Assert.Equal(station1, railway.From);
        Assert.Equal(station2, railway.To);
        Assert.Equal(duration, railway.Duration);

        railway = map.Railways.SingleOrDefault(
            x => x.From == station1 && x.To == station2 && x.Id == railwayId);
        Assert.NotNull(railway);
    }


    [Fact]
    public void CreateInvalidRailway()
    {
        const int lineId = 20;
        const int lineId2 = 21;
        const int stationId1 = 78;
        const int stationId2 = 79;
        const int stationId3 = 80;
        const int invalidStationId1 = 179;
        const int invalidStationId2 = 180;
        var testPoints = new List<Position>()
        {
            new(-2, 34),
            new(0, 2.3f),
            new(4.3f, -23)
        };
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line = map.CreateLine("UwU", lineId);
        var line2 = map.CreateLine("(*)v(*)", lineId2);
        var station1 = map.CreateStation(lineId, "u", stationId1);
        var station2 = map.CreateStation(lineId, "o", stationId2);
        var station3 = map.CreateStation(lineId2, "i", stationId3);

        Assert.Throws<InvalidOperationException>(
            () => map.CreateRailway(lineId, invalidStationId1, stationId2, duration));
        Assert.Throws<InvalidOperationException>(
            () => map.CreateRailway(lineId, stationId2, invalidStationId1, duration));
        Assert.Throws<InvalidOperationException>(
            () => map.CreateRailway(lineId, stationId2, stationId2, duration));
        Assert.Throws<InvalidOperationException>(
            () => map.CreateRailway(lineId, invalidStationId1, stationId3, duration));
    }


    [Fact]
    public void CreateTransfer()
    {
        const int lineId = 20;
        const int lineId2 = 21;
        const int stationId1 = 78;
        const int stationId2 = 79;
        const int railwayId = 3;
        var testPoints = new List<Position>()
        {
            new(-2, 34),
            new(0, 2.3f),
            new(4.3f, -23)
        };
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line = map.CreateLine("UwU", lineId);
        var line2 = map.CreateLine("(*)v(*)", lineId2);
        var station1 = map.CreateStation(lineId, "u", stationId1);
        var station2 = map.CreateStation(lineId2, "o", stationId2);

        var transfer = map.CreateTransfer(stationId1, stationId2, duration,
            railwayId);
        Assert.Equal(station1, transfer.From);
        Assert.Equal(station2, transfer.To);
        Assert.Equal(duration, transfer.Duration);

        transfer = map.Transfers.SingleOrDefault(
            x => x.From == station1 && x.To == station2 && x.Id == railwayId);
        Assert.NotNull(transfer);
    }


    [Fact]
    public void CreateInvalidTransfer()
    {
        const int lineId = 20;
        const int stationId1 = 78;
        const int stationId2 = 79;
        const int invalidStationId1 = 179;
        var testPoints = new List<Position>()
        {
            new(-2, 34),
            new(0, 2.3f),
            new(4.3f, -23)
        };
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line = map.CreateLine("UwU", lineId);
        var station1 = map.CreateStation(lineId, "u", stationId1);
        var station2 = map.CreateStation(lineId, "o", stationId2);

        Assert.Throws<InvalidOperationException>(
            () => map.CreateTransfer(invalidStationId1, stationId2, duration));
        Assert.Throws<InvalidOperationException>(
            () => map.CreateTransfer(stationId2, invalidStationId1, duration));
        Assert.Throws<InvalidOperationException>(
            () => map.CreateTransfer(stationId2, stationId2, duration));
    }


    [Fact]
    public void RemoveLine()
    {
        const int lineId1 = 10;
        const int lineId2 = 11;
        const int station1Id = 20;
        const int station2Id = 21;
        const int station3Id = 22;
        const int station4Id = 23;
        const int railway1Id = 34;
        const int railway2Id = 35;
        const int transfer1Id = 34;
        const int transfer2Id = 35;
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line1 = map.CreateLine("UwU", lineId1);
        var line2 = map.CreateLine("UwU", lineId2);
        var station1 = map.CreateStation(lineId1, "u_u", station1Id);
        var station2 = map.CreateStation(lineId1, "-u-", station2Id);
        var station3 = map.CreateStation(lineId2, "0-0", station3Id);
        var station4 = map.CreateStation(lineId2, "0w0", station4Id);

        var railway1 = map.CreateRailway(lineId1, station1Id, station2Id,
            duration, railway1Id);

        var railway2 = map.CreateRailway(lineId2, station3Id, station4Id,
            duration, railway2Id);

        var transfer1 = map.CreateTransfer(station1Id, station2Id,
            duration, transfer1Id);

        var transfer2 = map.CreateTransfer(station3Id, station4Id,
            duration, transfer2Id);

        map.RemoveLine(lineId1);

        Assert.DoesNotContain(station1, map.Stations);
        Assert.DoesNotContain(station2, map.Stations);
        Assert.DoesNotContain(railway1, map.Railways);
        Assert.DoesNotContain(transfer1, map.Transfers);
        Assert.DoesNotContain(line1, map.Lines);
        Assert.Contains(station3, map.Stations);
        Assert.Contains(station4, map.Stations);
        Assert.Contains(railway2, map.Railways);
        Assert.Contains(transfer2, map.Transfers);
        Assert.Contains(line2, map.Lines);
    }


    [Fact]
    public void RemoveStation()
    {
        const int lineId1 = 10;
        const int station1Id = 20;
        const int station2Id = 21;
        const int station3Id = 22;
        const int railway1Id = 34;
        const int railway2Id = 35;
        const int transfer1Id = 34;
        const int transfer2Id = 35;
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        map.CreateLine("UwU", lineId1);
        var station1 = map.CreateStation(lineId1, "u_u", station1Id);
        map.CreateStation(lineId1, "-u-", station2Id);
        map.CreateStation(lineId1, "0-0", station3Id);

        var railway1 = map.CreateRailway(lineId1, station1Id, station2Id,
            duration, railway1Id);

        var railway2 = map.CreateRailway(lineId1, station2Id, station3Id,
            duration, railway2Id);

        var transfer1 = map.CreateTransfer(station1Id, station2Id,
            duration, transfer1Id);

        var transfer2 = map.CreateTransfer(station2Id, station3Id,
            duration, transfer2Id);

        map.RemoveStation(station1Id);

        Assert.DoesNotContain(station1, map.Stations);
        Assert.DoesNotContain(railway1, map.Railways);
        Assert.DoesNotContain(transfer1, map.Transfers);
        Assert.Contains(railway2, map.Railways);
        Assert.Contains(transfer2, map.Transfers);
    }


    [Fact]
    public void RemoveRailway()
    {
        const int lineId1 = 10;
        const int station1Id = 20;
        const int station2Id = 21;
        const int railway1Id = 34;
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line1 = map.CreateLine("UwU", lineId1);
        var station1 = map.CreateStation(lineId1, "u_u", station1Id);
        var station2 = map.CreateStation(lineId1, "-u-", station2Id);

        var railway1 = map.CreateRailway(lineId1, station1Id, station2Id,
            duration, railway1Id);

        map.RemoveRailway(railway1Id);
        Assert.DoesNotContain(railway1, map.Railways);
    }


    [Fact]
    public void RemoveTransfer()
    {
        const int lineId1 = 10;
        const int station1Id = 20;
        const int station2Id = 21;
        const int transfer1Id = 34;
        var duration = new StaticDuration { StaticSeconds = 200 };

        var map = new Map("-u-");
        var line1 = map.CreateLine("UwU", lineId1);
        var station1 = map.CreateStation(lineId1, "u_u", station1Id);
        var station2 = map.CreateStation(lineId1, "-u-", station2Id);

        var transfer1 = map.CreateTransfer(station1Id, station2Id,
            duration, transfer1Id);

        map.RemoveTransfer(transfer1Id);
        Assert.DoesNotContain(transfer1, map.Transfers);
    }
}

