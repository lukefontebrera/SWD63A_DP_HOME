using Subscriber.Models;
using Subscriber.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure GCP settings
builder.Services.Configure<GCPSettings>(
    builder.Configuration.GetSection("GCP"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SubscriberService as a hosted service
builder.Services.AddHostedService<SubscriberService>();

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
