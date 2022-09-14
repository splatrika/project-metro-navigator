using System;
namespace Splatrika.MetroNavigator.Source.ViewModels;

public class ListElement
{
    public string Name { get; set; }
    public string Controller { get; set; }
    public int ElementId { get; set; }

    public ListElement(string name, string controller, int elementId)
    {
        Name = name;
        Controller = controller;
        ElementId = elementId;
    }
}

