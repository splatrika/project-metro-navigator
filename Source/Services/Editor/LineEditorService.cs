using Splatrika.MetroNavigator.Source.Interfaces;

namespace Splatrika.MetroNavigator.Source.Services.Editor;

public class LineEditorService : EditorService<LineDto>
{
    public const string DefaultName = "Unnamed line";


    public LineEditorService(IMapRepository repository,
        IMapAppearanceService appearance)
    {
        throw new NotImplementedException();
    }


    public override async Task<int> Create(int mapId)
    {
        throw new NotImplementedException();
    }


    public override async Task Delete(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }


    public override async Task<LineDto> Get(int mapId, int elementId)
    {
        throw new NotImplementedException();
    }


    public override async Task<int> Update(LineDto dto)
    {
        throw new NotImplementedException();
    }
}

