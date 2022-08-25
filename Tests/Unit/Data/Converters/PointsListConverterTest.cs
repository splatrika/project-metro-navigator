using System;
using Splatrika.MetroNavigator.Source.Data.Converters;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Tests.Unit.Data.Converters;

public class PointsListConverterTest
{
    [Fact]
    public void ConvertFromValidSource()
    {
        var source = "2;3|8,4;-2|-10000;-200,3";
        var result = PointsListConverter.Convert(source);
        Assert.Equal(3, result.Count);
        Assert.Equal(2, result[0].Left);
        Assert.Equal(8.4, result[1].Left, 0.001f);
        Assert.Equal(-10000, result[2].Left);
        Assert.Equal(3, result[0].Top);
        Assert.Equal(-2, result[1].Top);
        Assert.Equal(-200.3, result[2].Top, 0.001f);
    }

    [Fact]
    public void ConvertFromInvalidSource()
    {
        var source = "-2,3|2,5;2|2.3;3";
        Assert.Throws<InvalidCastException>(() =>
        {
            var result = PointsListConverter.Convert(source);
        });
    }

    [Fact]
    public void ConvertFromSourceWithInvalidFloats()
    {
        var source = "2.3;34.2|2,5;2|2.3;3";
        Assert.Throws<InvalidCastException>(() =>
        {
            var result = PointsListConverter.Convert(source);
        });
    }

    [Fact]
    public void ConvertFromEmptySource()
    {
        var source = "";
        var result = PointsListConverter.Convert(source);
        Assert.Empty(result);
    }

    [Fact]
    public void ConvertFromValue()
    {
        var value = new List<Position>()
        {
            new(10, 5),
            new(2, 5.4f),
            new(-2, -3.42f),
            new(20, -323)
        };
        var result = PointsListConverter.Convert(value);
        Assert.Equal("10;5|2;5,4|-2;-3,42|20;-323", result);
    }
}

