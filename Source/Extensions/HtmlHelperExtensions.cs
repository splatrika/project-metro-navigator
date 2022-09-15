using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Extensions;

public static class HtmlHelperExtensions
{
    public static string GetCaptionStyle(this IHtmlHelper _,
        StationAppearance appearance)
    {
        var captionWidth = 100f;
        var captionOffset = appearance.Caption.Offset;
        var style = new StringBuilder();
        if (appearance.Caption.TextAligin == TextAlign.Right)
        {
            captionOffset = new(
                captionOffset.Left - captionWidth,
                captionOffset.Top);
            style.Append("text-align: right; ");
        }
        else if (appearance.Caption.TextAligin == TextAlign.Center)
        {
            captionOffset = new(
                captionOffset.Left - captionWidth / 2,
                captionOffset.Top);
            style.Append("text-align: center; ");
        }
        style.Append($"top: {captionOffset.Top}px; ");
        style.Append($"left: {captionOffset.Left}px; ");
        style.Append($"width: {captionWidth}px");
        return style.ToString();
    }


    public static string AsPixels(this IHtmlHelper _, float value)
    {
        return $"{value}px";
    }


}

