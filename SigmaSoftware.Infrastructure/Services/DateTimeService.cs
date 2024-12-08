using SigmaSoftware.Application.Common.Interfaces;

namespace SigmaSoftware.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
    public DateTime Today => DateTime.Today;
}