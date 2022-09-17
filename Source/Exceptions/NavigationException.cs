namespace Splatrika.MetroNavigator.Source.Exceptions;

public class NavigationException : Exception
{
    public NavigationException(string from, string to)
        : base($"Unable to get route from {from} to {to}")
    {
    }
}

