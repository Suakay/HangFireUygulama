using HangFireUygulama.Context;
using HangFireUygulama.Models;

namespace HangFireUygulama.Service
{
    public class LoggingService : ILoggingService
    {
        private readonly AppDbContext _dbContext;

        public LoggingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogMessage(string message)
        {
            var logMessage = new Message { MessageContent = message, CreatedAt = DateTime.UtcNow };
            _dbContext.Messages.Add(logMessage);
            _dbContext.SaveChanges();
        }
       
    }
}
