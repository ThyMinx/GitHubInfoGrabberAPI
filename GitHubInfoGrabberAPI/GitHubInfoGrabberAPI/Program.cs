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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "GitHub Info Grabber API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseSwagger();
//app.UseDeveloperExceptionPage();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", "GitHub Info Grabber API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors(AllowMySite);

app.UseAuthorization();

app.MapControllers();

app.Run();
