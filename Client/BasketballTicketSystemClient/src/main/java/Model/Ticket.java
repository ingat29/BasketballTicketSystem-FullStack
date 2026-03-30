package Model;

public class Ticket extends Entity<Integer> {
    private Match match;
    private Customer customer;
    private int numberOfSeats;

    public Ticket(Integer id, Match match, Customer customer, int numberOfSeats) {
        super(id);
        this.match = match;
        this.customer = customer;
        this.numberOfSeats = numberOfSeats;
    }

    public Match getMatch() { return this.match; }
    public void setMatch(Match match) { this.match = match; }

    public Customer getCustomer() { return customer; }
    public void setCustomer(Customer customer) { this.customer = customer; }

    public int getNumberOfSeats() { return numberOfSeats; }
    public void setNumberOfSeats(int numberOfSeats) { this.numberOfSeats = numberOfSeats; }
}
