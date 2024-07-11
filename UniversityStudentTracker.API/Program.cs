using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using UniversityStudentTracker.API.Contexts;
using UniversityStudentTracker.API.Mappings;
using UniversityStudentTracker.API.Repositories;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Serilog
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Ensure that all levels are captured
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Logging.ClearProviders();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentMetrics Performance API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "OAuth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING");

// Log the connection string
logger.Information("Retrieved connection string", connectionString);

// Add DbContexts with logging
builder.Services.AddDbContext<StudentPerformance>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
    options.UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>());
});

builder.Services.AddDbContext<StudentAuth>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
    options.UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>());
});

builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("StudentMetrics")
    .AddEntityFrameworkStores<StudentAuth>().AddDefaultTokenProviders();

// Register repositories
builder.Services.AddScoped<IStudySessionInterface, StudySessionRepository>();
builder.Services.AddScoped<IBreakInterface, BreakRepository>();
builder.Services.AddScoped<IPredictionInterface, PredictionRepository>();
builder.Services.AddScoped<IStudentMetricsInterface, StudentMetricsRepository>();
builder.Services.AddScoped<IAdminInterface, AdminRepository>();

// Register services
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<StudySessionService>();
builder.Services.AddScoped<BreakService>();
builder.Services.AddScoped<PredictionService>();
builder.Services.AddScoped<StudentMetricsService>();
builder.Services.AddScoped<AdminService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Serve the Swagger UI at the app's root
    });
}

// Use CORS policy
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var testLogger = app.Services.GetRequiredService<ILogger<Program>>();
testLogger.LogInformation("Test log entry to verify logging setup.");

app.Run();