using Microsoft.OpenApi.Models;
using VerticalSliceMinimalApi.API.Endpoints;
using VerticalSliceMinimalApi.API.Middleware;
using VerticalSliceMinimalApi.Application;
using VerticalSliceMinimalApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "VerticalSlice - MinimalApi API",
        Description = "Account Microservice built using net8.0 and C#",
        Contact = new OpenApiContact
        {
            Name = "Ever Echeverri LinkedIn",
            Email = "everecheverri@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/ever-alonso-echeverri-velasquez-39956414a/")
        },
        License = new OpenApiLicense
        {
            Name = "Github",
            Url = new Uri("https://github.com/EverEcheverri")
        }
    });
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseSwagger();
}

// Register endpoints
AccountsEndpoints.MapAccountsEndpoints(app);

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.Run();
