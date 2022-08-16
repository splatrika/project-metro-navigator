using System;
namespace Source.Entities;

public class Transfer
{
    public int Id { get; set; }
    public Station From { get; set; }
    public Station To { get; set; }

    public Transfer(int id, Station from, Station to)
    {
        Id = id;
        From = from;
        To = to;
    }
}

