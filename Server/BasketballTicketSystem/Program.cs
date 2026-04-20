using System;
using System.Threading;

namespace BasketballTicketSystem {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("--- Basketball Ticket System Server ---");
            Console.WriteLine("Initializing databases...");

            IEmployeeRepository employeeRepo = new EmployeeDBRepository();
            IMatchRepository matchRepo = new MatchDBRepository();
            ITicketRepository ticketRepo = new TicketDBRepository();
            ICustomerRepository customerRepo = new CustomerDBRepository();
            ITeamRepository teamRepo = new TeamDBRepository();
            IStadiumRepository stadiumRepo = new StadiumDBRepository();

            ServerApp server = new ServerApp(employeeRepo, matchRepo, ticketRepo, customerRepo, teamRepo, stadiumRepo);

            Thread serverThread = new Thread(() => server.Start());
            serverThread.Start();

            Console.WriteLine("Server is running in the background.");
            Console.WriteLine("Press [ENTER] to forcefully shut down the terminal...");

            Console.ReadLine();
        }
    }
}