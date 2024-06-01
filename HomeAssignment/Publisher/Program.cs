using Publisher.Models;
using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure GCP settings
builder.Services.Configure<GCPSettings>(
    builder.Configuration.GetSection("GCP"));

// Add services to the container
builder.Services.AddSingleton<PublisherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
