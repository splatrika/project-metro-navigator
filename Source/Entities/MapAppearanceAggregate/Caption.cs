using System;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public record Caption(Position Offset, TextAligin TextAligin)
{
    public static Caption Default => new(new Position(), TextAligin.Left);

    private Caption() : this(Default) { } //Required by EF
}

public enum TextAligin
{
    Left,
    Center,
    Right
}