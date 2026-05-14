using System.Collections.Generic;
using System.Linq;
using NLog;

public class TicketDBRepository : ITicketRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public List<ITicket> FindTicketsByMatchId(int matchId) {
        using (var context = new BasketballContext()) {
            return context.Tickets.Where(t => t.matchId == matchId).Cast<ITicket>().ToList();
        }
    }

    public ITicket Add(ITicket ticket) {
        using (var context = new BasketballContext()) {
            context.Tickets.Add((Ticket)ticket);
            context.SaveChanges();
        }
        return ticket;
    }

    public ITicket FindById(int id) {
        using (var context = new BasketballContext()) {
            return context.Tickets.Find(id);
        }
    }

    public List<ITicket> FindAll() {
        using (var context = new BasketballContext()) {
            return context.Tickets.Cast<ITicket>().ToList();
        }
    }

    public ITicket Update(ITicket ticket) {
        using (var context = new BasketballContext()) {
            context.Tickets.Update((Ticket)ticket);
            context.SaveChanges();
        }
        return ticket;
    }

    public ITicket Delete(int id) {
        using (var context = new BasketballContext()) {
            var ticket = context.Tickets.Find(id);
            if (ticket != null) {
                context.Tickets.Remove(ticket);
                context.SaveChanges();
            }
            return ticket;
        }
    }
}