using Microsoft.EntityFrameworkCore;
using SigmaSoftware.Domain.Entities;

namespace SigmaSoftware.Application.Common.Interfaces;

public interface ISigmaDbContext
{
    DbSet<Candidate> Candidate { get; }

}
