using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Splatrika.MetroNavigator.Source.Entities;

namespace Splatrika.MetroNavigator.Source.Data.Converters;

public class PointsListConverter : ValueConverter<List<Position>, string>
{
    public PointsListConverter()
        : base(
            v => Convert(v),
            v => Convert(v))
    {
    }

    private const string PointsSeperator = "|";
    private const string AxisSeparator = ";";

    public static string Convert(List<Position> value)
    {
        const int averageDataSize = 8;
        var builder = new StringBuilder(value.Count * averageDataSize);
        for (var i = 0; i < value.Count; i++)
        {
            var point = value[i];
            builder.Append($"{point.Left}{AxisSeparator}{point.Top}");
            if (i != value.Count - 1)
            {
                builder.Append(PointsSeperator);
            }
        }
        return builder.ToString();
    }


    public static List<Position> Convert(string source)
    {
        if (source == "") return new();
        var data = source.Split(PointsSeperator);
        var result = new List<Position>(data.Length);
        foreach (var pointData in data)
        {
            var values = pointData.Split(AxisSeparator);
            if (values.Length != 2) throw new InvalidCastException();
            var expectedLength = values[0].Length + values[1].Length + AxisSeparator.Length;
            if (pointData.Length != expectedLength) throw new InvalidCastException();
            var left = 0f;
            var top = 0f;
            var ok = float.TryParse(values[0], out left)
                  && float.TryParse(values[1], out top);
            if (!ok)
            {
                throw new InvalidCastException();
            }
            result.Add(new(left, top));
        }
        return result;
    }
}

