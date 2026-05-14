using Microsoft.EntityFrameworkCore;
using System.Configuration;

public class BasketballContext : DbContext {
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            // We retrieve the connection string from existing App.config
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BasketballTicketsDB"].ConnectionString;

            // Pomelo requires us to specify the MySQL server version
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }
}