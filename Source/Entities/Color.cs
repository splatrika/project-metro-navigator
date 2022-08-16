namespace Source.Entities;

public class Color
{
    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }
    public float Alpha { get; set; }

    public string GetHex()
    {
        return $"#{Red:X2}{Green:X2}{Blue:X2}{Alpha:X2}";
    }
}

