using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}