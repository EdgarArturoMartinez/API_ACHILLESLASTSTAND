using AchillesLastStand.Application.Interfaces;
using AchillesLastStand.Domain.Entities;
using AchillesLastStand.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AchillesLastStand.Infrastructure.Repositories
{
    // INFRASTRUCTURE LAYER - REPOSITORY IMPLEMENTATION
    // This is the concrete implementation of IJobApplicationRepository interface.
    // Following SOLID: Dependency Inversion Principle - this implements the abstraction defined in Application layer.
    // Following Repository Pattern - encapsulates data access logic using EF Core.
    // This class is registered in DI container and injected wherever IJobApplicationRepository is needed.
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor injection of DbContext
        // DbContext is injected by ASP.NET Core's dependency injection container
        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all job applications
        // AsNoTracking improves read performance when we don't need change tracking
        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await _context.JobApplications
                .AsNoTracking()
                .ToListAsync();
        }

        // Retrieve a single job application by ID
        // Returns null if not found (nullable JobApplication?)
        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _context.JobApplications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // Create a new job application
        // Add method stages the entity; SaveChangesAsync commits to database
        public async Task<JobApplication> CreateAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        // Update an existing job application
        // Update method marks the entity as modified; SaveChangesAsync commits changes
        public async Task UpdateAsync(JobApplication jobApplication)
        {
            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync();
        }

        // Delete a job application by ID
        // We need to retrieve it first, then remove it
        public async Task DeleteAsync(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
                await _context.SaveChangesAsync();
            }
        }

        // Check if a job application exists by ID
        // AnyAsync is more efficient than retrieving the full entity
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.JobApplications.AnyAsync(x => x.Id == id);
        }

        // FILTERING/SEARCH - Implements query with optional filters
        // Uses LINQ to filter data based on optional parameters
        // AsNoTracking for read-only performance optimization
        // NOTE: EF.Functions.Like() is used for case-insensitive partial matching (translates to SQL LIKE)
        public async Task<IEnumerable<JobApplication>> SearchAsync(string? company = null, string? role = null)
        {
            // Start with all job applications
            IQueryable<JobApplication> query = _context.JobApplications.AsNoTracking();

            // Apply company filter if provided (case-insensitive, partial match)
            // SQL Server LIKE is case-insensitive by default
            if (!string.IsNullOrWhiteSpace(company))
            {
                query = query.Where(j => EF.Functions.Like(j.Company, $"%{company}%"));
            }

            // Apply role filter if provided (case-insensitive, partial match)
            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(j => EF.Functions.Like(j.Role, $"%{role}%"));
            }

            // Execute query and return results
            return await query.ToListAsync();
        }
    }
}