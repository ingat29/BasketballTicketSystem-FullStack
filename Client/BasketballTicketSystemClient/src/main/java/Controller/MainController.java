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
        return matchService.getAllAvailableMatches();
    }

    public void buyTicket(Match match, Integer customerId, int seats) throws Exception {
        if (loggedInEmployee == null) {
            throw new Exception("You must be logged in to sell tickets!");
        }
        if (match == null) {
            throw new Exception("Please select a match first!");
        }
        if (customerId == null || customerId == 0) {
            throw new Exception("Customer id cannot be empty or zero!");
        }
        if (seats <= 0) {
            throw new Exception("Number of seats must be greater than 0!");
        }

        ticketService.buyTicket(match, customerId, seats);
    }
}