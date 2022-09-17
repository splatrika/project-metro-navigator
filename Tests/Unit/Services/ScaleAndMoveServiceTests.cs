using Splatrika.MetroNavigator.Source.Services;
using Splatrika.MetroNavigator.Source.ViewModels;

namespace Splatrika.MetroNavigator.Unit.Services;

public class ScaleAndMoveServiceTests
{
    [Fact]
    public void GetDefault()
    {
        var service = new ScaleAndMoveService();
        var actual = service.Get();
        Assert.Equal(ScaleAndMove.Default, actual);
    }


    [Fact]
    public void GetSetted()
    {
        var service = new ScaleAndMoveService();
        var excepted = new ScaleAndMove(Top: -10, Left: 122, Scale: 2);
        service.Set(excepted);
        var actual = service.Get();
        Assert.Equal(excepted, actual);
    }
}

