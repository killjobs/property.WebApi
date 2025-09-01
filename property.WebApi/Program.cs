using property.Infrastructure.Context;
using property.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using property.Application.Services;
using MongoDB.Bson.Serialization.Conventions;
using property.Infrastructure.Configurations;
using property.Domain.Interfaces.Repositories;
using property.Domain.Interfaces.Services;
using propertyImage.Application.Services;
using property.Application.Handlers;
using property.Domain.Interfaces.Handlers;

var builder = WebApplication.CreateBuilder(args);

var conventionPack = new ConventionPack
{
    new SnakeCaseElementNameConvention()
};
ConventionRegistry.Register("SnakeCase", conventionPack, t => true);

// Add services to the container.
string mongoConnection = builder.Configuration.GetConnectionString("MongoDb");
string mongoDatabase = builder.Configuration["MongodBDatabase"];

builder.Services.AddSingleton(new MongoDbContext(mongoConnection, mongoDatabase));
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
builder.Services.AddScoped<IImageHandler, ImageHandler>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen( c=>{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Property API",
        Version = "v1",
        Description = "API to manage properties"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowReact");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Property API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
