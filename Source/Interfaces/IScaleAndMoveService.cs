using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Source.Interfaces;


public interface IScaleAndMoveService
{
    ScaleAndMove Get();
    void Set(ScaleAndMove value);
}
