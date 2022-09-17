using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Source.Services;

public class ScaleAndMoveService : IScaleAndMoveService
{
    private ScaleAndMove _value;


    public ScaleAndMoveService()
    {
        _value = ScaleAndMove.Default;
    }


    public ScaleAndMove Get()
    {
        return _value;
    }


    public void Set(ScaleAndMove value)
    {
        _value = value;
    }
}

