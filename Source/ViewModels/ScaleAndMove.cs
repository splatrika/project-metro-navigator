namespace Splatrika.MetroNavigator.Source.ViewModels;

public record ScaleAndMove(float Top, float Left, float Scale)
{
    public static ScaleAndMove Default => new(Top: 0, Left: 0, Scale: 1);
}

