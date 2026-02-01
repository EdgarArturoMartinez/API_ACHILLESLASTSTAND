using AchillesLastStand.Domain.Entities;

namespace AchillesLastStand.Application.Interfaces
{
    // APPLICATION LAYER - REPOSITORY INTERFACE
    // This is an abstraction (interface) that defines the contract for data access operations.
    // Following SOLID: Dependency Inversion Principle (DIP) - high-level modules depend on abstractions.
    // Following Design Pattern: Repository Pattern - abstracts data access logic.
    // The Infrastructure layer will implement this interface with the actual database code (EF Core).
    public interface IJobApplicationRepository
    {
        // Async methods are best practice for I/O operations (database calls)
        // Task<T> represents an asynchronous operation that returns a value
        // Task represents an asynchronous operation that doesn't return a value

        Task<IEnumerable<JobApplication>> GetAllAsync();
        Task<JobApplication?> GetByIdAsync(int id);
        Task<JobApplication> CreateAsync(JobApplication jobApplication);
        Task UpdateAsync(JobApplication jobApplication);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}