using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.ViewModels;

public class Navigator
{
    public int MapId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public Station[]? Route { get; set; }
}

