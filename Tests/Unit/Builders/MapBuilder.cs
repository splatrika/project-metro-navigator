using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Moq;
using System.Linq.Expressions;
using System.Reflection;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class MapBuilder
{
    public static Map WithItems(int mapId,
        List<Line>? linesReference = null,
        List<Station>? stationsReference = null,
        List<Railway>? railwaysReference = null,
        List<Transfer>? transfersReference = null,
        string name = "Map",
        CreateCallbacks? createCallbacks = null,
        RemoveCallbacks? removeCallbacks = null)
    {
        var mock = new Mock<Map>();

        if (createCallbacks != null)
            MockCreate(mock, (CreateCallbacks)createCallbacks);
        if (removeCallbacks != null)
            MockRemove(mock, (RemoveCallbacks)removeCallbacks);

        var linesList = linesReference ?? new();
        var stationsList = stationsReference ?? new();
        var railwaysList = railwaysReference ?? new();
        var transfersList = transfersReference ?? new();

        mock.SetupGet(m => m.Lines)
            .Returns(() => linesList.AsReadOnly());

        mock.SetupGet(m => m.Stations)
            .Returns(() => stationsList.AsReadOnly());

        mock.SetupGet(m => m.Railways)
            .Returns(() => railwaysList.AsReadOnly());

        mock.SetupGet(m => m.Transfers)
            .Returns(() => transfersList.AsReadOnly());

        return mock.Object;
    }


    private static void MockCreate(Mock<Map> mock,
        CreateCallbacks callbacks)
    {
        mock.Setup(m => m.CreateLine(It.IsAny<string>(), 0))
            .Returns<string>(name =>
            {
                var line = new Line(name);
                if (callbacks.CreateLine != null)
                {
                    callbacks.CreateLine(line);
                }
                return line;
            });

        mock.Setup(m => m.CreateStation(It.IsAny<int>(), It.IsAny<string>(), 0))
            .Returns<int, string>((lineId, name) =>
            {
                var line = new Line("line", lineId);
                var station = new Station(name, line);
                if (callbacks.CreateStaiton != null)
                {
                    callbacks.CreateStaiton(station);
                }
                return station;
            });

        mock.Setup(m => m.CreateRailway(It.IsAny<int>(),
                                        It.IsAny<int>(),
                                        It.IsAny<int>(),
                                        It.IsAny<DurationFactor>(),
                                        0))
            .Returns<int, int, int, DurationFactor>(
            (lineId, fromId, toId, duration) =>
            {
                var line = new Line("line", lineId);
                var from = new Station("from", line, fromId);
                var to = new Station("to", line, toId);

                var railway = new Railway(from, to, duration);
                if (callbacks.CreateRailway != null)
                {
                    callbacks.CreateRailway(railway);
                }
                return railway;
            });

        mock.Setup(m => m.CreateTransfer(It.IsAny<int>(),
                                         It.IsAny<int>(),
                                         It.IsAny<DurationFactor>(),
                                         0))
            .Returns<int, int, DurationFactor>(
            (fromId, toId, duration) =>
            {
                var line = new Line("line", -1);
                var from = new Station("from", line, fromId);
                var to = new Station("to", line, toId);

                var transfer = new Transfer(from, to, duration);
                if (callbacks.CreateTransfer != null)
                {
                    callbacks.CreateTransfer(transfer);
                }
                return transfer;
            });
    }


    private static void MockRemove(Mock<Map> mock,
        RemoveCallbacks callbacks)
    {
        var expressionCallbacks = new Dictionary<
            Expression<Action<Map>>,
            Action<int>?>();

        expressionCallbacks.Add(m => m.RemoveLine(It.IsAny<int>()),
            callbacks.RemoveLine);

        expressionCallbacks.Add(m => m.RemoveStation(It.IsAny<int>()),
            callbacks.RemoveStation);

        expressionCallbacks.Add(m => m.RemoveTransfer(It.IsAny<int>()),
            callbacks.RemoveTransfer);

        expressionCallbacks.Add(m => m.RemoveRailway(It.IsAny<int>()),
            callbacks.RemoveRailway);


        foreach (var expression in expressionCallbacks.Keys)
        {
            var callback = expressionCallbacks[expression];

            mock.Setup(expression)
                .Callback<int>(id =>
                {
                    if (callback != null) callback(id);
                });
        }

    }


    public struct CreateCallbacks
    {
        public Action<Line>? CreateLine;
        public Action<Station>? CreateStaiton;
        public Action<Railway>? CreateRailway;
        public Action<Transfer>? CreateTransfer;
    }


    public struct RemoveCallbacks
    {
        public Action<int>? RemoveLine;
        public Action<int>? RemoveStation;
        public Action<int>? RemoveTransfer;
        public Action<int>? RemoveRailway;
    }
}

