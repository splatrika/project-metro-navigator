using Splatrika.MetroNavigator.Source.Data;
using Splatrika.MetroNavigator.Source.Entities;
using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();