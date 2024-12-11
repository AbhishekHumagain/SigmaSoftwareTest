using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SigmaSoftware.Domain.Entities;

namespace SigmaSoftware.Infrastructure.Persistence
{
    public class SigmaDbContextInitializer(ILogger<SigmaDbContextInitializer> logger, SigmaSigmaDbContext context)
    {
        public async Task InitialiseAsync()
        {
            try
            {
                if (context.Database.IsNpgsql())  // If using PostgreSQL
                {
                    await context.Database.MigrateAsync();  // Ensure latest migrations are applied
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            if (!context.Candidate.Any())  // Check if the Candidates table is empty
            {
                var candidates = new[]
                {
                    new Candidate
                    {
                        Email = "john.doe@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        PhoneNumber = "1234567890",
                        CallTimeInterval = "9 AM - 11 AM",
                        LinkedInProfileUrl = "https://linkedin.com/in/johndoe",
                        GitHubProfileUrl = "https://github.com/johndoe",
                        Comment = "Looking for opportunities in software development.",
                    },
                    new Candidate
                    {
                        Email = "jane.smith@example.com",
                        FirstName = "Jane",
                        LastName = "Smith",
                        PhoneNumber = "0987654321",
                        CallTimeInterval = "2 PM - 4 PM",
                        LinkedInProfileUrl = "https://linkedin.com/in/janesmith",
                        GitHubProfileUrl = "https://github.com/janesmith",
                        Comment = "Experienced in project management and team leadership.",
                    }
                };

                // Add seed candidates
                await context.Candidate.AddRangeAsync(candidates);
                await context.SaveChangesAsync(); // Save changes to the database
            }
            else
            {
                Console.WriteLine("Candidates table is already populated.");
            }
        }
    }
}
