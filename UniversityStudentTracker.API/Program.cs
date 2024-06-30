using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING");

builder.Services.AddDbContext<StudentPerformance>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddDbContext<StudentAuth>(options =>
{
    options.UseSqlServer(connectionString);
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

app.MapControllers();

app.Run();