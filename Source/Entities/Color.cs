namespace Splatrika.MetroNavigator.Source.Entities;

public record Color(float Red = 0, float Green = 0, float Blue = 0, float Alpha = 1)
{
    public static Color White => new(1, 1, 1, 1);
    public static Color RedColor => new(1, 0, 0, 1);

    public string GetHex()
    {
        var red = (int)(Red * 255);
        var green = (int)(Green * 255);
        var blue = (int)(Blue * 255);
        var alpha = (int)(Alpha * 255);
        return $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}";
    }
}
