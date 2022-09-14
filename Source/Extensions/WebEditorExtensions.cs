using Microsoft.AspNetCore.Mvc.Rendering;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor;

namespace Splatrika.MetroNavigator.Source.Extensions;

public static class WebEditorExtensions
{
    public static void MapEditor(this WebApplication application)
    {
        application.MapAreaControllerRoute(
            name: "Editor",
            areaName: "Editor",
            pattern: "/map-editor/{mapId}/{controller}/{elementId?}/{action=Edit}");

        application.MapAreaControllerRoute(
            name: "EditorNoElement",
            areaName: "Editor",
            pattern: "/map-editor/{mapId}/{controller}/{action}");

        application.MapAreaControllerRoute(
            name: "EditorNoSelected",
            areaName: "Editor",
            pattern: "/map-editor/{mapId}/{action=Index}",
            defaults: new { controller = "NoSelected" });
    }


    public static IEnumerable<SelectListItem> GetStationItems(this IHtmlHelper _,
        Map map, int selectedId)
    {
        var result = new List<SelectListItem>(map.Stations.Count);
        foreach (var station in map.Stations)
        {
            result.Add(new(
                text: station.Name,
                value: station.Id.ToString(),
                selected: station.Id == selectedId,
                disabled: false));
        }
        return result.AsReadOnly();
    }


    public static void AddWebEditor(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEditorViewService, EditorViewService>();
    }
}

