using Core.Extensions;
using Core.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Core
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class EFContext : DbContext
    {
        public static readonly LoggerFactory _loggerFactory
        = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public DbSet<ChatMember> ChatMember { get; set; }
        public DbSet<ChatUser> ChatUser { get; set; }
        public DbSet<RegisterHealthCardInfo> RegisterHealthCardInfo { get; set; }
        public DbSet<RegisterHealthCardErrorLog> RegisterHealthCardErrorLog { get; set; }


        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(_loggerFactory)
                    .UseMySql(ConfigurationManager.AppSettings["HCMysqlConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
