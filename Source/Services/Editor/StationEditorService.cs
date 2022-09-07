using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class StationEditorService : EditorService<StationEditorDto> // todo fix
{
    public const string DefaultName = "Unnamed station";

    private readonly IMapAppearanceService _appearanceService;


    public StationEditorService(IMapRepository repository,
        IMapAppearanceService appearance) : base(repository)
    {
        _appearanceService = appearance;
    }

    public override async Task<int> Create(int mapId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithLines(mapId, true);
        if (map.Lines.Count == 0)
        {
            throw new EditorException("There is no lines to assign station");
        }
        var line = map.Lines.First();
        var station = map.CreateStation(line.Id, DefaultName);
        await _repository.SaveChangesAsync();
        return station.Id;
    }


    public override async Task Delete(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithStation(mapId, elementId, true);
        map.RemoveStation(elementId);
        await _repository.SaveChangesAsync();
        await _appearanceService.CleanUpStation(mapId, elementId);
    }


    public override async Task<StationEditorDto> Get(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithStationsAndLines(mapId, false);
        var station = GetElement(map.Stations, elementId);
        var appearance = await _appearanceService.GetStation(mapId, elementId);
        return new()
        {
            MapId = mapId,
            ElementId = elementId,
            Name = station.Name,
            LineId = station.Line.Id,
            Position = appearance.Position,
            Caption = appearance.Caption
        };
    }


    public override async Task<int> Update(StationEditorDto dto)
    {
        await CheckMap(dto.MapId);
        var map = await _repository.GetFull(dto.MapId, true);
        var station = GetElement(map.Stations, dto.ElementId);
        if (dto.LineId != station.Line.Id)
        {
            map.RemoveStation(dto.ElementId);
            await _appearanceService.CleanUpStation(dto.MapId, dto.ElementId);
            station = map.CreateStation(dto.LineId, dto.Name);
            await _repository.SaveChangesAsync();
            await _appearanceService.UpdateStation(dto.MapId, station.Id,
                dto.Position, dto.Caption);
            return station.Id;
        }
        station.Name = dto.Name;
        await _repository.SaveChangesAsync();
        await _appearanceService.UpdateStation(dto.MapId, dto.ElementId,
            dto.Position, dto.Caption);
        return station.Id;
    }
}

