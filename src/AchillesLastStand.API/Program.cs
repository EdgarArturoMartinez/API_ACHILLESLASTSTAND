using AchillesLastStand.Application.Interfaces;
using AchillesLastStand.Infrastructure.Data;
using AchillesLastStand.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ADD CONTROLLERS - Required for API controllers to work
builder.Services.AddControllers();

// ADD SWAGGER - API documentation and testing UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DATABASE CONFIGURATION - Entity Framework Core with SQL Server
// This registers the DbContext in the DI container with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        )
    )
);

// REPOSITORY PATTERN - Dependency Injection
// Register the repository implementation with Scoped lifetime
// Scoped = one instance per HTTP request (best practice for DbContext-dependent services)
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

// CORS CONFIGURATION - Allow frontend applications to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ENABLE CORS
app.UseCors("AllowAll");

// CONFIGURE SWAGGER UI (only in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// MAP CONTROLLERS - This makes your JobApplicationsController work!
app.MapControllers();

app.Run();