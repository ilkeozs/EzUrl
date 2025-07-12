using Carter;
using EzUrl.Data;
using EzUrl.Services;
using EzUrl.Services.Qr;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCarter();

// Add services
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IQrService, QrService>();

// Automatic url cleanup service
builder.Services.AddHostedService<ExpiredUrlCleanupService>();

var app = builder.Build();

app.MapOpenApi();

app.MapCarter();

// Options for Scalar
app.MapScalarApiReference(options => options
    .WithTitle("EzUrl API")
    .WithTheme(ScalarTheme.Kepler)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient));

app.Run();