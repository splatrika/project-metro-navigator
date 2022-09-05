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

        return mock.Object;
    }
}

