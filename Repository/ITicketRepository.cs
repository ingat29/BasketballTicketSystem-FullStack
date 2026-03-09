using System.Collections.Generic;

public interface ITicketRepository : IRepository<int, ITicket>
{
    List<ITicket> FindTicketsByMatchId(int matchId);
}