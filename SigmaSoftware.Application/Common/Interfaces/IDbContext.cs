using Microsoft.EntityFrameworkCore;

namespace SigmaSoftware.Application.Common.Interfaces;

public interface IDbContext
{
    DbSet<Domain.Entities.Candidate> Candidate { get; }

}
