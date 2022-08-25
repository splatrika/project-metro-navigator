using System;
using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

namespace Splatrika.MetroNavigator.Source.Data;

#nullable disable

public class ApplicationContext : DbContext
{
    public DbSet<Map> Maps { get; set; }
    public DbSet<MapAppearance> MapAppearance { get; set; }
    public DbSet<Line> Lines { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Railway> Railways { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<LineAppearance> LineAppearance { get; set; }
    public DbSet<StationAppearance> StationAppearance { get; set; }
    public DbSet<RailwayAppearance> RailwayAppearances { get; set; }


    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationContext).Assembly);
    }
}

