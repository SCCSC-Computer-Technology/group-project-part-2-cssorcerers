using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using SportsAPI.Interfaces;
using SportsAPI.Repositories;
using SportsData;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    Console.WriteLine("Default output formatters: ");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaformmater = formatter as OutputFormatter;
        if (mediaformmater == null)
        {
            Console.WriteLine($" {formatter.GetType().Name}.");
        }
        else
        {
            Console.WriteLine(" {0}, Media Types: {1}", arg0: mediaformmater.GetType().Name, arg1: string.Join(", ",
                mediaformmater.SupportedMediaTypes));
        }
    }
}).AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CPT 206 Service Web API", Version = "v1" });
});

builder.Services.AddDbContext<SportsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SportsDB")));

builder.Services.AddScoped<INFLRepository, NFLRepository>();
builder.Services.AddScoped<INBARepository, NBARepository>();
builder.Services.AddScoped<IF1Repository, F1Repository>();
builder.Services.AddScoped<IPremierRepository, PremierRepository>();
builder.Services.AddScoped<ICSGORepository, CSGORepository>();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sports API");
        c.SupportedSubmitMethods(new[] { SubmitMethod.Get });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
