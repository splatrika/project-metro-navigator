using Splatrika.MetroNavigator.Source.Data;
using Splatrika.MetroNavigator.Source.Entities;
using Microsoft.EntityFrameworkCore;
using Splatrika.MetroNavigator.Source.Entities.MapAggregate;
using Splatrika.MetroNavigator.Source.Entities.MapAppearanceAggregate;
using Splatrika.MetroNavigator.Source.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var options = new DbContextOptionsBuilder<ApplicationContext>()
    .UseSqlServer(builder.Configuration.GetConnectionString("ApplicationConnection"))
    .Options;

using var context = new ApplicationContext(options);

var repository = new MapRepository(context);
var map = await repository.GetWithStationsAsync(1, lineId: 3);

return;

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();