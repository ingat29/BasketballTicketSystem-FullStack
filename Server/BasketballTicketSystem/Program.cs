using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasketballTicketSystem {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container (Dependency Injection)
            // This tells the framework to give a Controller the right repository when it asks for it.
            builder.Services.AddControllers();

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowReactClient", policy => {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddSignalR();

            builder.Services.AddScoped<IMatchRepository, MatchDBRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeDBRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketDBRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerDBRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamDBRepository>();
            builder.Services.AddScoped<IStadiumRepository, StadiumDBRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseRouting();

            //Configure CORS to allow requests from the React client
            app.UseCors("AllowReactClient");

            // This maps incoming HTTP requests to your Controllers
            app.MapControllers();

            app.MapHub<BasketballTicketSystem.Hubs.NotificationHub>("/notificationHub");

            // Start the Web Server (By default, it will listen on localhost:5000)
            app.Run();
        }
    }
}