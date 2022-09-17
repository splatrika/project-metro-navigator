using Microsoft.AspNetCore.Mvc;
using Moq;
using Splatrika.MetroNavigator.Source.Areas.Navigator;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.ViewModels;
using Splatrika.MetroNavigator.Tests.Unit.Builders;

namespace Splatrika.MetroNavigator.Tests.Unit.Controllers;

#nullable disable

public class NavigatorControllerTests
{
    [Fact]
    public async Task DefaultMap()
    {
        var map = MapBuilder.WithItems(mapId: 10);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetAsync(map.Id, It.IsAny<bool>()));
        var service = new Mock<INavigationService>().Object;

        var controller = new NavigatorController(repository, service);

        var result = await controller.Index(map: null, from: null, to: null);
        Assert.IsType<RedirectToActionResult>(result);
        if (result is RedirectToActionResult redirect)
        {
            Assert.NotNull(redirect.RouteValues);
            Assert.True(redirect.RouteValues.ContainsKey("map"));
        }
    }

    [Fact]
    public async Task WithoutRoute()
    {
        var map = MapBuilder.WithItems(mapId: 10);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetAsync(map.Id, It.IsAny<bool>()));
        var service = new Mock<INavigationService>().Object;

        var controller = new NavigatorController(repository, service);

        var result = await controller.Index(map: map.Name, from: null, to: null);
        Assert.IsType<ViewResult>(result);
        var view = (ViewResult)result;
        Assert.NotNull(view.Model);
        Assert.IsType<Navigation>(view.Model);
        var model = (Navigation)view.Model;
        Assert.Equal(map.Id, model.MapId);
        Assert.Null(model.From);
        Assert.Null(model.To);
        Assert.Null(model.Route);
    }


    [Fact]
    public async Task GetRoute()
    {
        var station1 = new Station("station1", line: null, id: 1);
        var station2 = new Station("station2", line: null, id: 2);
        var map = MapBuilder.WithItems(mapId: 101,
            stationsReference: new() { station1, station2 });
        var exceptedRoute = new Station[] { station1, station2 };
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetWithStations(map.Id, It.IsAny<bool>()));
        var service = NavigationServiceBuilder.WithStaticRoute(exceptedRoute);

        var controller = new NavigatorController(repository, service);
        var result = await controller
            .Index(map.Name, station1.Name, station2.Name);

        Assert.IsType<ViewResult>(result);
        var view = (ViewResult)result;
        Assert.NotNull(view.Model);
        Assert.IsType<Navigation>(view.Model);
        var model = (Navigation)view.Model;
        Assert.Equal(map.Id, model.MapId);
        Assert.Equal(station1.Name, model.From);
        Assert.Equal(station2.Name, model.To);
        Assert.True(exceptedRoute.SequenceEqual(model.Route));
    }


    [Fact]
    public async Task UnknownMap()
    {
        var map = MapBuilder.WithItems(mapId: 10);
        var repository = MapRepositoryBuilder.WithSingleMap(map,
            allowedQuery: x => x.GetAsync(map.Id, It.IsAny<bool>()));
        var service = new Mock<INavigationService>().Object;
        var unknownName = map.Name + "aaaa";

        var controller = new NavigatorController(repository, service);

        var result = await controller.Index(map: unknownName, from: null, to: null);
        Assert.IsType<NotFoundResult>(result);
    }
}
