using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Api.Middleware;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Application.Services;
using PruebaTecnicaKibernum.Application.Validators;
using PruebaTecnicaKibernum.Infrastructure.DataContext;
using PruebaTecnicaKibernum.Infrastructure.ExternalServices;
using PruebaTecnicaKibernum.Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateHiringRequestValidator>();

//DI
builder.Services.AddHttpClient<IRickAndMortyService, RickAndMortyService>(client =>
{
    client.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
});

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IHiringRequestRepository, HiringRequestRepository>();
builder.Services.AddScoped<IHiringRequestService, HiringRequestService>();

builder.Services.AddHealthChecks();

//ConnectionString
var lConnectionString = "Data Source=app"; 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(lConnectionString));


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

//Error middleware
app.UseMiddleware<ExceptionMiddleware>();

//Health Check
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (lContext, lReport) =>
    {
        var response = new
        {
            Status = lReport.Status.ToString(),
            Timestamp = DateTime.UtcNow
        };

        lContext.Response.ContentType = "application/json";
        await lContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
