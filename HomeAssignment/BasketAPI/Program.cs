using BasketAPI.Models;
using BasketAPI.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BasketDatabaseSettings>(
    builder.Configuration.GetSection("BasketDatabaseSettings"));

builder.Services.Configure<OrderDatabaseSettings>(
    builder.Configuration.GetSection("OrderDatabaseSettings"));

builder.Services.AddSingleton<BasketService>();
builder.Services.AddSingleton<OrderService>();

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
