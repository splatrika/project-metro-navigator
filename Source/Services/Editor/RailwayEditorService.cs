
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class RailwayEditorService : EditorService<RailwayEditorDto>
{
    public static DurationFactor DefaultDuration => new StaticDuration(120);

    private readonly IMapAppearanceService _appearanceService;


    public RailwayEditorService(IMapRepository repository,
        IMapAppearanceService appearance) : base(repository)
    {
        _appearanceService = appearance;
    }


    public override async Task<int> Create(int mapId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithStationsAndLines(mapId);

        var from = map.Stations.FirstOrDefault();
        var to = map.Stations.FirstOrDefault(x =>
           x.Id != from.Id &&
           x.Line.Id == from.Line.Id);
        if (from == null || to == null)
        {
            throw new EditorException("There is no stations to create" +
                "railway between them");
        }

        var station = map
            .CreateRailway(from.Line.Id, from.Id, to.Id, DefaultDuration);
        await _repository.SaveChangesAsync();
        return station.Id;
    }


    public override async Task Delete(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithRailway(mapId, elementId);
        map.RemoveRailway(elementId);
        await _repository.SaveChangesAsync();

        await _appearanceService.CleanUpRailway(mapId, elementId);
    }


    public override async Task<RailwayEditorDto> Get(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository
            .GetWithRailwayAndStationsBetween(mapId, elementId, tracking: false);
        var transfer = GetElement(map.Railways, elementId);
        var appearance = await _appearanceService.GetRailway(mapId, elementId);

        return new()
        {
            MapId = mapId,
            ElementId = elementId,
            FromId = transfer.From.Id,
            ToId = transfer.To.Id,
            Points = appearance.Points
        };
    }


    public override async Task<int> Update(RailwayEditorDto dto)
    {
        await CheckMap(dto.MapId);
        var map = await _repository.GetFull(dto.MapId);
        var railway = GetElement(map.Railways, dto.ElementId);

        var stationsChanged = railway.From.Id != dto.FromId ||
            railway.To.Id != dto.ToId;
        if (stationsChanged)
        {
            map.RemoveRailway(railway.Id);
            await _appearanceService.CleanUpRailway(dto.MapId, railway.Id);
            var from = map.Stations.First(x => x.Id == dto.FromId);

            railway = map.CreateRailway(from.Line.Id, dto.FromId,
                dto.ToId, railway.Duration);
            await _repository.SaveChangesAsync();
            await _appearanceService
                .UpdateRailway(dto.MapId, railway.Id, dto.Points);
            return railway.Id;
        }

        await _appearanceService
            .UpdateRailway(dto.MapId, railway.Id, dto.Points);
        await _repository.SaveChangesAsync();
        return railway.Id;
    }
}

