using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Exceptions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class LineEditorService : EditorService<LineDto>
{
    public const string DefaultName = "Unnamed line";

    private readonly IMapAppearanceService _appearanceService;


    public LineEditorService(IMapRepository repository,
        IMapAppearanceService appearance) : base(repository)
    {
        _appearanceService = appearance;
    }


    public override async Task<int> Create(int mapId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetAsync(mapId);
        var line = map.CreateLine(DefaultName);
        await _repository.SaveChangesAsync();
        return line.Id;
    }


    public override async Task Delete(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithLine(mapId, elementId);
        map.RemoveLine(elementId);
        await _appearanceService.CleanUpLine(mapId, elementId);
        await _repository.SaveChangesAsync();
    }


    public override async Task<LineDto> Get(int mapId, int elementId)
    {
        await CheckMap(mapId);
        var map = await _repository.GetWithLine(mapId, elementId);
        var line = GetElement(map.Lines, elementId);
        var appearance = await _appearanceService.GetLine(mapId, elementId);

        return new LineDto
        {
            MapId = mapId,
            ElementId = elementId,
            Name = line.Name,
            Color = appearance.Color
        };
    }


    public override async Task<int> Update(LineDto dto)
    {
        await CheckMap(dto.MapId);
        var map = await _repository.GetWithLine(dto.MapId, dto.ElementId);
        var line = GetElement(map.Lines, dto.ElementId);
        line.Name = dto.Name;
        await _repository.SaveChangesAsync();

        await _appearanceService
            .UpdateLine(dto.MapId, dto.ElementId, dto.Color);

        return line.Id;
    }
}

