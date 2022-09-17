using System.Linq.Expressions;
using Moq;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class MapRepositoryBuilder
{
    public static IMapRepository WithSingleMap(Map map,
        Expression<Func<IMapRepository, Task<Map>>> allowedQuery,
        Action? saveCallback = null)
    {
        var mock = new Mock<IMapRepository>();

        mock.Setup(allowedQuery)
            .Returns(Task.FromResult(map));

        mock.Setup(m => m.ContainsAsync(map.Id))
            .Returns(Task.FromResult(true));

        mock.Setup(m => m.ContainsAsync(It.IsNotIn(map.Id)))
            .Returns(Task.FromResult(false));

        mock.Setup(m => m.SaveChangesAsync())
            .Callback(() => saveCallback?.Invoke())
            .Returns(Task.CompletedTask);

        mock.Setup(m => m.GetList())
            .Returns(Task.FromResult(new List<Map>() { map }));

        return mock.Object;
    }
}

