using System.Linq.Expressions;
using Moq;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class MapAppearanceRepositoryBuilder
{
    public static IMapAppearanceRepository WithSingle(MapAppearance appearance,
        Expression<Func<IMapAppearanceRepository, Task<MapAppearance>>> allowedQuery,
        Action? saveCallback = null,
        Action<MapAppearance>? addCallback = null)
    {
        var mock = new Mock<IMapAppearanceRepository>();

        mock.Setup(allowedQuery)
            .Returns(Task.FromResult(appearance));

        mock.Setup(m => m.SaveChangesAsync())
            .Callback(() => saveCallback?.Invoke())
            .Returns(Task.CompletedTask);

        mock.Setup(m => m.ContainsForMapAsync(appearance.MapId))
            .Returns(Task.FromResult(true));

        mock.Setup(m => m.ContainsForMapAsync(It.IsNotIn(appearance.MapId)))
            .Returns(Task.FromResult(false));

        mock.Setup(m => m.AddAsync(It.IsAny<MapAppearance>()))
            .Callback<MapAppearance>(
                appearance => addCallback?.Invoke(appearance))
            .Returns(Task.CompletedTask);
        
        return mock.Object;
    }


    public static IMapAppearanceRepository Empty(
        Action? saveCallback = null,
        Action<MapAppearance>? addCallback = null)
    {
        var mock = new Mock<IMapAppearanceRepository>();

        mock.Setup(m => m.ContainsForMapAsync(It.IsAny<int>()))
            .Returns(Task.FromResult(false));

        mock.Setup(m => m.SaveChangesAsync())
            .Callback(() => saveCallback?.Invoke())
            .Returns(Task.CompletedTask);

        mock.Setup(m => m.AddAsync(It.IsAny<MapAppearance>()))
            .Callback<MapAppearance>(
                appearance => addCallback?.Invoke(appearance))
            .Returns(Task.CompletedTask);

        return mock.Object;
    }
}

