using System;

public interface ITicket
{
    int ticketId { get; set; }
    int matchId { get; set; }
    int customerId { get; set; }
    int numberOfSeats { get; set; }
}