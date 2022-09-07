using System.Reflection;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Extensions;

public static class EditorExtensions
{
    public static void AddEditorServices(this WebApplicationBuilder builder)
    {
        var serviceTypes = typeof(EditorExtensions).Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(y => y == typeof(IEditorService)))
            .Where(x => !x.IsAbstract)
            .ToArray();

        foreach (var type in serviceTypes)
        {
            builder.Services.AddScoped(type);
        }
    }


    public static void MapEditor(this WebApplication application)
    {
        application.MapControllerRoute(
            name: "Admin",
            pattern: "map-editor/{mapId}/{controller}/{elementIf}/{action=Edit}",
            defaults: new { Area = "Editor" });
    }
}

