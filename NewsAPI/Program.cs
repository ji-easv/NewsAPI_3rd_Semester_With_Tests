using System.Data;
using System.Data.Common;
using NewsAPI;
using Core.Services;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (builder.Environment.IsEnvironment("Testing"))
{
    // SQLite in-memory database only exists while the connection is open
    var connection = new SqliteConnection("DataSource=:memory:");
    connection.Open();
    builder.Services.AddSingleton<IDbConnection>(container => connection);
}
else
{
    builder.Services.AddSingleton<IDbConnection>(container =>
    {
        var connection = new NpgsqlConnection(Utilities.ProperlyFormattedConnectionString);
        connection.Open();
        return connection;
    });
}

builder.Services.AddScoped<ArticleRepository>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();