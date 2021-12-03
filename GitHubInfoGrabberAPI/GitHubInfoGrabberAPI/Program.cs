var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
