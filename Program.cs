using Microsoft.EntityFrameworkCore;
using Employeee.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:10000");

// Add services
builder.Services.AddControllers();

builder.Services.AddDbContext<EmployeeDbcContexet>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ADD CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// USE CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();