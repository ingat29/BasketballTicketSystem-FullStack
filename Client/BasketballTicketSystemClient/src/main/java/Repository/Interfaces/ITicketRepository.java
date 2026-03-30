package Repository.Interfaces;
import Model.Ticket;
import java.util.List;

public interface ITicketRepository extends IRepository<Integer, Ticket> {
    List<Ticket> findTicketsByMatchId(int matchId);
}
