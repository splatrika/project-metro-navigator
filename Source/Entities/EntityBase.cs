using System;
namespace Splatrika.MetroNavigator.Source.Entities;

public class EntityBase
{
    public int Id { get; private set; }

    protected EntityBase() { } // Required by EF

    public EntityBase(int id = 0)
    {
        Id = id;
    }
}

