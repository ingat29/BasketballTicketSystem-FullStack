package Repository;
import Model.Ticket;
import Repository.Interfaces.ITicketRepository;

import java.util.List;
import java.util.stream.Collectors;

public class TicketInMemoryRepository extends InMemoryRepository<Integer, Ticket> implements ITicketRepository {
    @Override
    public List<Ticket> findTicketsByMatchId(int matchId) {
        return entities.values().stream()
                .filter(t -> t.getMatch().getId() == matchId)
                .collect(Collectors.toList());
    }
}