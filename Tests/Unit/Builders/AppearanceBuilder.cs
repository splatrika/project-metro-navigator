using System.Linq.Expressions;
using Moq;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class AppearanceBuilder
{
    public static IMapAppearanceService Empty(
        UpdateCallbacks? updateCallbacks = null,
        CleanUpCallbacks? cleanUpCallbacks = null)
    {
        var mock = BaseSetup(updateCallbacks, cleanUpCallbacks);
        MockDefaultGets(mock);
        return mock.Object;
    }


    public static IMapAppearanceService WithConfiguredLine(int mapId,
        LineAppearance lineAppearance,
        UpdateCallbacks? updateCallbacks = null,
        CleanUpCallbacks? cleanUpCallbacks = null)
    {
        var mock = BaseSetup(updateCallbacks, cleanUpCallbacks);

        mock.Setup(m => m.GetLine(mapId, lineAppearance.LineId))
            .Returns(Task.FromResult(lineAppearance));

        MockDefaultGets(mock, excludeStationId: lineAppearance.LineId);

        return mock.Object;
    }


    public static IMapAppearanceService WithConfiguredStation(int mapId,
        StationAppearance stationAppearance,
        UpdateCallbacks? updateCallbacks = null,
        CleanUpCallbacks? cleanUpCallbacks = null)
    {
        var mock = BaseSetup(updateCallbacks, cleanUpCallbacks);

        mock.Setup(m => m.GetStation(mapId, stationAppearance.StationId))
            .Returns(Task.FromResult(stationAppearance));

        MockDefaultGets(mock, excludeStationId: stationAppearance.StationId);

        return mock.Object;
    }


    public static IMapAppearanceService WithConfiguredRailway(int mapId,
        RailwayAppearance railwayAppearance,
        UpdateCallbacks? updateCallbacks = null,
        CleanUpCallbacks? cleanUpCallbacks = null)
    {
        var mock = BaseSetup(updateCallbacks, cleanUpCallbacks);

        mock.Setup(m => m.GetRailway(mapId, railwayAppearance.RailwayId))
            .Returns(Task.FromResult(railwayAppearance));

        MockDefaultGets(mock, excludeRailwayId: railwayAppearance.RailwayId);

        return mock.Object;
    }


    private static Mock<IMapAppearanceService> BaseSetup(
        UpdateCallbacks? updateCallbacks = null,
        CleanUpCallbacks? cleanUpCallbacks = null)
    {
        var mock = new Mock<IMapAppearanceService>();

        MockUpdates(mock, updateCallbacks ?? new());
        MockCleanUp(mock, cleanUpCallbacks ?? new());

        return mock;
    }


    private static void MockDefaultGets(Mock<IMapAppearanceService> mock,
        int? excludeStationId = null,
        int? excludeLineId = null,
        int? excludeRailwayId = null)
    {
        Expression<Func<IMapAppearanceService, Task<LineAppearance>>>
            getLineExpression =
                m => m.GetLine(It.IsAny<int>(), It.IsAny<int>());
        if (excludeLineId != null)
        {
            var id = (int)excludeLineId!;
            getLineExpression =
                m => m.GetLine(It.IsAny<int>(), It.IsNotIn(id));
        }

        Expression<Func<IMapAppearanceService, Task<StationAppearance>>>
            getStationExpression =
                m => m.GetStation(It.IsAny<int>(), It.IsAny<int>());
        if (excludeStationId != null)
        {
            var id = (int)excludeStationId!;
            getLineExpression =
                m => m.GetLine(It.IsAny<int>(), It.IsNotIn(id));
        }

        Expression<Func<IMapAppearanceService, Task<RailwayAppearance>>>
            getRailwayExpression =
                m => m.GetRailway(It.IsAny<int>(), It.IsAny<int>());
        if (excludeRailwayId != null)
        {
            var id = (int)excludeRailwayId!;
            getRailwayExpression =
                m => m.GetRailway(It.IsAny<int>(), It.IsNotIn(id));
        }

        mock.Setup(getLineExpression)
            .Returns<int, int>((_, id) =>
                Task.FromResult(new LineAppearance(id, color: new())));

        mock.Setup(getStationExpression)
            .Returns<int, int>((_, id) => Task.FromResult(
                new StationAppearance(id, new Position(), Caption.Default)));

        mock.Setup(getRailwayExpression)
            .Returns<int, int>((_, id) => Task.FromResult(
                new RailwayAppearance(id, points: new List<Position>())));
    }


    private static void MockUpdates(Mock<IMapAppearanceService> mock,
        UpdateCallbacks callbacks)
    {

        mock.Setup(m => m.UpdateStation(It.IsAny<int>(),
                                        It.IsAny<int>(),
                                        It.IsAny<Position>(),
                                        It.IsAny<Caption>()))
            .Returns<int, int, Position, Caption>(
            (mapId, stationId, position, caption) =>
            {
                if (callbacks.UpdateStationCallback != null)
                {
                    callbacks.UpdateStationCallback
                        (mapId, stationId, position, caption);
                }
                return Task.CompletedTask;
            });

        mock.Setup(m => m.UpdateLine(It.IsAny<int>(),
                                     It.IsAny<int>(),
                                     It.IsAny<Color>()))
            .Returns<int, int, Color>(
            (mapId, lineId, color) =>
            {
                if (callbacks.UpdateLineCallback != null)
                {
                    callbacks.UpdateLineCallback(mapId, lineId, color);
                }
                return Task.CompletedTask;
            });

        mock.Setup(m => m.UpdateRailway(It.IsAny<int>(),
                                        It.IsAny<int>(),
                                        It.IsAny<IEnumerable<Position>>()))
            .Returns<int, int, IEnumerable<Position>>(
            (mapId, railwayId, points) =>
            {
                if (callbacks.UpdateRailwayCallback != null)
                {
                    callbacks.UpdateRailwayCallback(mapId, railwayId, points);
                }
                return Task.CompletedTask;
            });
    }


    private static void MockCleanUp(Mock<IMapAppearanceService> mock,
        CleanUpCallbacks callbacks)
    {
        var callbacksList = new List<Action<int, int>?>()
        {
            callbacks.CleanUpLineCallback,
            callbacks.CleanUpStationCallback,
            callbacks.CleanUpRailwayCallback
        };
        var expressions = new List<Expression<Func<IMapAppearanceService, Task>>>()
        {
            m => m.CleanUpLine(It.IsAny<int>(), It.IsAny<int>()),
            m => m.CleanUpStation(It.IsAny<int>(), It.IsAny<int>()),
            m => m.CleanUpRailway(It.IsAny<int>(), It.IsAny<int>()),
        };
        for (var i = 0; i < callbacksList.Count; i++)
        {
            mock.Setup(expressions[i])
                .Returns<int, int>((mapId, elementId) =>
                {
                    if (callbacksList[i] != null)
                    {
                        callbacksList[i]!.Invoke(mapId, elementId);
                    }
                    return Task.CompletedTask;
                });
        }
    }


    public struct CleanUpCallbacks
    {
        public Action<int, int>? CleanUpStationCallback;
        public Action<int, int>? CleanUpLineCallback;
        public Action<int, int>? CleanUpRailwayCallback;
    }


    public struct UpdateCallbacks
    {
        public Action<int, int, Position, Caption>? UpdateStationCallback;
        public Action<int, int, Color>? UpdateLineCallback;
        public Action<int, int, IEnumerable<Position>>? UpdateRailwayCallback;
    }
}

