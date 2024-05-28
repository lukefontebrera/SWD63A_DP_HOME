using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using CatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("SWD63ADPHOME"));

builder.Services.AddSingleton<MovieService>();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseCors(policy => policy.WithOrigins("http://localhost:5070")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();