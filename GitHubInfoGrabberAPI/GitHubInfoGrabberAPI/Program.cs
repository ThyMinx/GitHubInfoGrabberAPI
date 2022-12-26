using GitHubInfoGrabberAPI.Services;
using Microsoft.OpenApi.Models;

var AllowMySite = "_allowMySite";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowMySite,
        builder =>
        {
            builder.WithOrigins("http://thejamescairns.co.uk",
                "http://www.thejamescairns.co.uk",
                "https://thejamescairns.co.uk",
                "https://www.thejamescairns.co.uk",
                "http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddScoped<IGitHubInfoService, GitHubInfoService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => new OpenApiInfo
{
    Version = "v1",
    Title = "GitHub Info Grabber API",
    Description = "A .NET Core web API for grabbing info from GitHub."
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint("swagger/v1/swagger.json", "GitHub Info Grabber API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors(AllowMySite);

app.UseAuthorization();

app.MapControllers().RequireCors(AllowMySite);

app.Run();
