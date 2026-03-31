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

    public Ticket buyTicket(Match match, /* String customerName */Integer customerId, int numberOfSeats) throws Exception {
        //Check if there are enough seats
        if (match.getAvailableSeats() < numberOfSeats) {
            throw new Exception("Not enough available seats for this match!");
        }

        Customer customer = customerRepository.findById(customerId);
        if (customer == null) {
            return null;
            //Or throw error
        }

        match.setAvailableSeats(match.getAvailableSeats() - numberOfSeats);
        matchRepository.update(match);

        int newTicketId = ticketRepository.findAll().size() + 1;
        Ticket newTicket = new Ticket(newTicketId, match, customer, numberOfSeats);
        ticketRepository.add(newTicket);

        return newTicket;
    }
}