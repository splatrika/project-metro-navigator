using System;
using System.Reflection;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Tests.Unit;

public static class EntityUtility
{
    public static void ChangeId(EntityBase instance, int id)
    {
        var property = typeof(EntityBase).GetProperty("Id",
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!;
        property.SetValue(instance, id);
    }
}

