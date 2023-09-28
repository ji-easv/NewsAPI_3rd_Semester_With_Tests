using System.Data;
using NewsAPI;
using Core.Services;
using Infrastructure;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDbConnection>(container =>
{
    var connection = new NpgsqlConnection(Utilities.ProperlyFormattedConnectionString);
    connection.Open();
    return connection;
});

builder.Services.AddScoped<ArticleRepository>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSpaStaticFiles(conf => conf.RootPath = "../NewsfeedFrontend/dist/newsfeed-frontend");

var app = builder.Build();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

// Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseSpa(conf => conf.Options.SourcePath = "../NewsfeedFrontend/dist/newsfeed-frontend");

app.MapControllers();
app.Run();