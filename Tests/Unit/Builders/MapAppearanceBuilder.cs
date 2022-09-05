using System;
using System.Linq.Expressions;
using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class MapAppearanceBuilder
{
    public static MapAppearance WithItems(int mapId,
        List<LineAppearance>? linesReference = null,
        List<StationAppearance>? stationsReference = null,
        List<RailwayAppearance>? railwaysReference = null,
        CleanUpCallbacks? cleanUpCallbacks = null,
        UpdateCallbacks? updateCallbacks = null)
    {
        var mock = new Mock<MapAppearance>();
        if (cleanUpCallbacks != null)
            MockCleanUp(mock, (CleanUpCallbacks)cleanUpCallbacks);
        if (updateCallbacks != null)
            MockUpdates(mock, (UpdateCallbacks)updateCallbacks);

        mock.Setup(m => m.GetLine(It.IsAny<int>()))
            .Returns<int>(id =>
            {
                var @default = LineAppearance.GetDefault(id);
                var list = linesReference ?? new();
                var found = list.SingleOrDefault(x => x.LineId == id)
                    ?? @default;
                return found;
            });

        mock.Setup(m => m.GetStation(It.IsAny<int>()))
            .Returns<int>(id =>
            {
                var @default = StationAppearance.GetDefault(id);
                var list = stationsReference ?? new();
                var found = list.SingleOrDefault(x => x.Id == id)
                    ?? @default;
                return found;
            });

        mock.Setup(m => m.GetRailway(It.IsAny<int>()))
            .Returns<int>(id =>
            {
                var @default = RailwayAppearance.GetDefault(id);
                var list = railwaysReference ?? new();
                var found = list.SingleOrDefault(x => x.Id == id)
                    ?? @default;
                return found;
            });

        return mock.Object;
    }


    public static void MockUpdates(Mock<MapAppearance> mock,
        UpdateCallbacks callbacks)
    {
        mock.Setup(m => m.UpdateLine(It.IsAny<int>(), It.IsAny<Color>()))
            .Callback<int, Color>((id, color) =>
            {
                var created = new LineAppearance(id, color);
                callbacks.UpdateLine?.Invoke(created);
            });

        mock.Setup(m => m.UpdateStation(It.IsAny<int>(),
                                        It.IsAny<Position>(),
                                        It.IsAny<Caption>()))
            .Callback<int, Position, Caption>((id, position, caption) =>
            {
                var created = new StationAppearance(id, position, caption);
                callbacks.UpdateStation?.Invoke(created);
            });

        mock.Setup(m => m.UpdateRailway(It.IsAny<int>(), It.IsAny<List<Position>>()))
            .Callback<int, List<Position>>((id, points) =>
            {
                var created = new RailwayAppearance(id, points);
                callbacks.UpdateRailway?.Invoke(id, points);
            });
    }


    public static void MockCleanUp(Mock<MapAppearance> mock,
        CleanUpCallbacks callbacks)
    {
        var expressionCallbacks = new Dictionary<
            Expression<Action<MapAppearance>>,
            Action<int>?>();

        expressionCallbacks.Add(x => x.CleanUpStation(It.IsAny<int>()),
            callbacks.CleanUpStation);
        expressionCallbacks.Add(x => x.CleanUpLine(It.IsAny<int>()),
            callbacks.CleanUpLine);
        expressionCallbacks.Add(x => x.CleanUpRailway(It.IsAny<int>()),
            callbacks.CleanUpRailway);

        foreach (var expression in expressionCallbacks.Keys)
        {
            mock.Setup(expression)
                .Callback<int>(id =>
                {
                    var callback = expressionCallbacks[expression];
                    if (callback != null) callback.Invoke(id);
                });
        }
    }


    public struct CleanUpCallbacks
    {
        public Action<int>? CleanUpStation;
        public Action<int>? CleanUpLine;
        public Action<int>? CleanUpRailway;
    }


    public struct UpdateCallbacks
    {
        public Action<StationAppearance>? UpdateStation;
        public Action<LineAppearance>? UpdateLine;
        public Action<RailwayAppearance>? UpdateRailway;
    }
}

