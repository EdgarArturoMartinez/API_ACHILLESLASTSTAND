using AchillesLastStand.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AchillesLastStand.Infrastructure.Data
{
    // INFRASTRUCTURE LAYER - DbContext
    // This is the EF Core DbContext that manages database connections and entity tracking.
    // Following Clean Architecture: Infrastructure depends on Domain (entities) but Domain doesn't know about EF Core.
    // DbContext is the Unit of Work pattern implementation in EF Core.
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepts DbContextOptions for dependency injection
        // This allows us to configure the connection string externally (in Program.cs)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet represents a table in the database
        // Each DbSet<T> corresponds to a table, and each T instance is a row
        public DbSet<JobApplication> JobApplications { get; set; }

        // OnModelCreating is used to configure entity mappings using Fluent API
        // This is best practice over Data Annotations for complex configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure JobApplication entity
            modelBuilder.Entity<JobApplication>(entity =>
            {
                // Table name in database
                entity.ToTable("JobApplications");

                // Primary key configuration
                entity.HasKey(e => e.Id);

                // Id is IDENTITY column (auto-increment)
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Configure Company property
                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(200);

                // Configure Role property
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(200);

                // Configure AppliedFromPlatform property
                entity.Property(e => e.AppliedFromPlatform)
                    .IsRequired()
                    .HasMaxLength(100);

                // Configure Status property
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                // Configure Payment property (nullable decimal)
                entity.Property(e => e.Payment)
                    .HasPrecision(18, 2); // decimal(18,2)

                // Configure Contact property (nullable)
                entity.Property(e => e.Contact)
                    .HasMaxLength(500);

                // Configure Observation property (nullable)
                entity.Property(e => e.Observation)
                    .HasMaxLength(1000);

                // Configure AppliedDate property
                entity.Property(e => e.AppliedDate)
                    .IsRequired();
            });
        }
    }
}