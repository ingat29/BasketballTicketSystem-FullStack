using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class TicketDBRepository : ITicketRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public List<ITicket> FindTicketsByMatchId(int matchId) {
        logger.Info($"Finding tickets for match ID: {matchId}");
        var list = new List<ITicket>();
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Tickets WHERE matchId = @matchId", connection);
            command.Parameters.AddWithValue("@matchId", matchId);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    list.Add(new Ticket(reader.GetInt32("ticketId"), reader.GetInt32("matchId"), reader.GetInt32("customerId"), reader.GetInt32("numberOfSeats")));
                }
            }
        }
        return list;
    }

    public ITicket Add(ITicket ticket) {
        //Ticket ticket = (Ticket)entity;
        logger.Info("Adding a new ticket sale.");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Tickets (matchId, customerId, numberOfSeats) VALUES (@match, @customer, @seats)", connection);
            command.Parameters.AddWithValue("@match", ticket.matchId);
            command.Parameters.AddWithValue("@customer", ticket.customerId);
            command.Parameters.AddWithValue("@seats", ticket.numberOfSeats);
            command.ExecuteNonQuery();
        }
        return ticket;
    }

    public ITicket FindById(int id) {
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Tickets WHERE ticketId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) return new Ticket(reader.GetInt32("ticketId"), reader.GetInt32("matchId"), reader.GetInt32("customerId"), reader.GetInt32("numberOfSeats"));
            }
        }
        return null;
    }

    public List<ITicket> FindAll() {
        var list = new List<ITicket>();
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Tickets", connection);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) list.Add(new Ticket(reader.GetInt32("ticketId"), reader.GetInt32("matchId"), reader.GetInt32("customerId"), reader.GetInt32("numberOfSeats")));
            }
        }
        return list;
    }

    public ITicket Update(ITicket ticket) {
        //Ticket ticket = (Ticket)entity;
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("UPDATE Tickets SET matchId = @match, customerId = @customer, numberOfSeats = @seats WHERE ticketId = @id", connection);
            command.Parameters.AddWithValue("@match", ticket.matchId);
            command.Parameters.AddWithValue("@customer", ticket.customerId);
            command.Parameters.AddWithValue("@seats", ticket.numberOfSeats);
            command.Parameters.AddWithValue("@id", ticket.ticketId);
            command.ExecuteNonQuery();
        }
        return ticket;
    }

    public ITicket Delete(int id) {
        ITicket deletedTicket = FindById(id);
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Tickets WHERE ticketId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        return deletedTicket;
    }
}