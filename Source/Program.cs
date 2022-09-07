using Splatrika.MetroNavigator.Source.Data;
using Splatrika.MetroNavigator.Source.Entities;
using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Extensions;
using Splatrika.MetroNavigator.Source.Interfaces;
using Splatrika.MetroNavigator.Source.Data.Repositories;
using Splatrika.MetroNavigator.Source.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationConnection")));



builder.Services.AddScoped<IMapRepository, MapRepository>();
builder.Services.AddScoped<IMapAppearanceRepository, MapAppearanceRepository>();
builder.Services.AddScoped<IMapAppearanceService, MapAppearanceService>();
builder.AddEditorServices();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");



app.Run();