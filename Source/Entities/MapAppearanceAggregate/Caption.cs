using System;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

public record Caption(Position Offset, TextAlign TextAligin)
{
    public static Caption Default => new(new Position(), TextAlign.Left);

    private Caption() : this(Default) { } //Required by EF
}

public enum TextAlign
{
    Left,
    Center,
    Right
}