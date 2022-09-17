using System;
using Moq;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Tests.Unit.Builders;

public static class NavigationServiceBuilder
{
    public static INavigationService WithStaticRoute(Station[] route)
    {
        var mock = new Mock<INavigationService>();
        mock.Setup(m => m.GetRoute(It.IsAny<int>(), It.IsAny<int>(),
                                   It.IsAny<int>()))
            .Returns(Task.FromResult( (Station[])route.Clone() ));
        return mock.Object;
    }
}

