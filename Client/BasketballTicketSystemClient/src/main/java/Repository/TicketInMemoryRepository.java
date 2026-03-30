package Repository;
import Model.Ticket;
import Repository.Interfaces.ITicketRepository;

import java.util.List;
import java.util.stream.Collectors;

public class TicketInMemoryRepository extends InMemoryRepository<Integer, Ticket> implements ITicketRepository {

    public TicketInMemoryRepository() {
        Model.Match m1 = new Model.Match(1, new Model.Team(1, "Lakers"), new Model.Team(2, "Bulls"), new Model.Stadium(1, "Crypto.com Arena", 20000), java.time.LocalDateTime.now(), 50.0f, 20000);

        this.add(new Model.Ticket(1, m1, new Model.Customer(1, "Customer Name 1"), 2));
        this.add(new Model.Ticket(2, m1, new Model.Customer(2, "Customer Name 2"), 4));
        this.add(new Model.Ticket(3, m1, new Model.Customer(3, "Customer Name 3"), 1));
        this.add(new Model.Ticket(4, m1, new Model.Customer(4, "Customer Name 4"), 5));
    }

    @Override
    public List<Ticket> findTicketsByMatchId(int matchId) {
        return entities.values().stream()
                .filter(t -> t.getMatch().getId() == matchId)
                .collect(Collectors.toList());
    }
}