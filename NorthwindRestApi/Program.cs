using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injektiolla välitetty tietokantatieto kontrollerille
builder.Services.AddDbContext<NorthwindOriginalContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("paikallinen")
    //builder.Configuration.GetConnectionString("pilvi")
    ));

//----------Cors määritys----------------
builder.Services.AddCors(options =>
{
        options.AddPolicy("all",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("all");

app.MapControllers();

app.Run();
