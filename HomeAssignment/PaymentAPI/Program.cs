using PaymentAPI.Models;
using PaymentAPI.Services;
using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Database settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("SWD63ADPHOME"));

// Configure GCP settings
builder.Services.Configure<GCPSettings>(
    builder.Configuration.GetSection("GCP"));

// Add services to the container
builder.Services.AddSingleton<PaymentService>();
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

app.UseCors(policy => policy.WithOrigins("http://localhost:5070")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
