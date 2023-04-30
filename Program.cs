using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SRI02_Api.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}); 

var conn = new SqliteConnection("Filename=:memory:");
conn.Open();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(conn);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
