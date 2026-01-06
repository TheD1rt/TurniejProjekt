using Microsoft.EntityFrameworkCore;
using HotChocolate.Execution.Configuration;
using Tournament.Data.Context;
using Tournament.Services;
using Tournament.Types;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<TournamentContext>(options =>
    options.UseSqlite("Data Source=tournament.db"));

builder.Services.AddScoped<TournamentService>();
builder.Services.AddScoped<JwtService>();


builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();


app.MapGraphQL();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TournamentContext>();
    db.Database.EnsureCreated();
}

app.Run();
