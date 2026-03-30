package Controller;

import Model.Employee;
import Model.Match;
import Service.EmployeeService;
import Service.MatchService;
import Service.TicketService;
import java.util.List;

public class MainController {
    private EmployeeService employeeService;
    private MatchService matchService;
    private TicketService ticketService;

    // Keeps track of the logged-in user
    private Employee loggedInEmployee = null;

    public MainController(EmployeeService employeeService, MatchService matchService, TicketService ticketService) {
        this.employeeService = employeeService;
        this.matchService = matchService;
        this.ticketService = ticketService;
    }

    public boolean login(String username, String password) {
        Employee emp = employeeService.login(username, password);
        if (emp != null) {
            this.loggedInEmployee = emp;
            return true;
        }
        return false;
    }

    public void logout() {
        this.loggedInEmployee = null;
    }

    public Employee getLoggedInEmployee() {
        return loggedInEmployee;
    }

    public List<Match> getAvailableMatches() {
        // Fetch matches from the service to display in the UI
        return matchService.getAllAvailableMatches();
    }

    public void buyTicket(Match match, String customerName, int seats) throws Exception {
        if (loggedInEmployee == null) {
            throw new Exception("You must be logged in to sell tickets!");
        }
        if (match == null) {
            throw new Exception("Please select a match first!");
        }
        if (customerName == null || customerName.trim().isEmpty()) {
            throw new Exception("Customer name cannot be empty!");
        }
        if (seats <= 0) {
            throw new Exception("Number of seats must be greater than 0!");
        }

        // Call the service to handle the complex business logic
        ticketService.buyTicket(match, customerName, seats);
    }
}