using System;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services;

namespace Splatrika.MetroNavigator.Source.Extensions;

public static class NavigatorApplicationExtensions
{
    public static void AddNavigator(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<INavigationService, NavigationService>();
        builder.Services.AddScoped<IScaleAndMoveService, ScaleAndMoveService>();
    }
}

