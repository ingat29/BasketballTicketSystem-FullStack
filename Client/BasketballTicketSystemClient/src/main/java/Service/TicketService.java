package Service;

import Model.Customer;
import Model.Match;
import Model.Ticket;
import Repository.Interfaces.ICustomerRepository;
import Repository.Interfaces.IMatchRepository;
import Repository.Interfaces.ITicketRepository;

public class TicketService {
    private ITicketRepository ticketRepository;
    private IMatchRepository matchRepository;
    private ICustomerRepository customerRepository;

    public TicketService(ITicketRepository ticketRepository, IMatchRepository matchRepository, ICustomerRepository customerRepository) {
        this.ticketRepository = ticketRepository;
        this.matchRepository = matchRepository;
        this.customerRepository = customerRepository;
    }

    public Ticket buyTicket(Match match, String customerName, int numberOfSeats) throws Exception {
        //Check if there are enough seats
        if (match.getAvailableSeats() < numberOfSeats) {
            throw new Exception("Not enough available seats for this match!");
        }

        //Find or create the customer
        Customer customer = customerRepository.findByName(customerName);
        if (customer == null) {
            // Generate a fake ID for the dummy repository
            int newCustomerId = customerRepository.findAll().size() + 1;
            customer = new Customer(newCustomerId, customerName);
            customerRepository.add(customer);
        }

        //Decrease the available seats and update the Match
        match.setAvailableSeats(match.getAvailableSeats() - numberOfSeats);
        matchRepository.update(match);

        //Create and save the new Ticket
        int newTicketId = ticketRepository.findAll().size() + 1;
        Ticket newTicket = new Ticket(newTicketId, match, customer, numberOfSeats);
        ticketRepository.add(newTicket);

        return newTicket;
    }
}