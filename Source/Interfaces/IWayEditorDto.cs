using System;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;

namespace Splatrika.MetroNavigator.Source.Interfaces;

public interface IWayEditorDto : IEditorDto
{
    int FromId { get; set; }
    int ToId { get; set; }
}

