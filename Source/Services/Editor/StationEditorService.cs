using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Services.Editor.Dto;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class StationEditorService : EditorService<StationEditorDto> // todo fix
{
    public const string DefaultName = "Unnamed station";


    public StationEditorService(IMapRepository repository,
        IMapAppearanceService appearance)
    {
        throw new NotImplementedException();
    }

    public override Task<int> Create(int mapId)
    {
        throw new NotImplementedException();
    }

    public override Task Delete(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }

    public override Task<StationEditorDto> Get(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }

    public override Task<int> Update(StationEditorDto dto)
    {
        throw new NotImplementedException();
    }
}

