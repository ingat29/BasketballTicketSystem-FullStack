package Service;

import Model.Match;
import Model.Ticket;
import Networking.ServerProxy;

public class TicketService {
    private ServerProxy proxy;

    public TicketService(ServerProxy proxy) {
        this.proxy = proxy;
    }

    public Ticket buyTicket(Match match, Integer customerId, int numberOfSeats) throws Exception {
        // The Proxy will throw an exception if there are not enough seats or the database rejects it
        boolean success = proxy.buyTicket(match.getId(), customerId, numberOfSeats);

        if (success) {
            // MainController doesn't use the returned Ticket object in its UI, so returning null is fine
            return null;
        }
        return null;
    }
}