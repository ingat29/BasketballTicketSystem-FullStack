using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class MatchDBRepository : IMatchRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public MatchDBRepository() { logger.Info("Initializing MatchDBRepository"); }

    public List<IMatch> FindAllAvailableMatchesOrderedDescending() {
        logger.Info("Fetching all available matches ordered descending by seats.");
        var matches = new List<IMatch>();

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM matches WHERE numberOfSeatsAvailable > 0 ORDER BY numberOfSeatsAvailable DESC", connection);

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    matches.Add(
                        new Match(
                            reader.GetInt32("matchId"),
                            reader.GetInt32("teamAId"),
                            reader.GetInt32("teamBId"),
                            reader.GetInt32("stadiumId"),
                            reader.GetInt32("numberOfSeatsAvailable"),
                            reader.GetFloat("ticketPrice")
                        )
                    );
                }
            }
        }
        return matches;
    }

    public List<IMatch> FindAvailableMatchesOrderedDescending(int minSeats) {
        logger.Info("Fetching all matches that have more than {minSeats} seats, ordered descending by seats.");
        var matches = new List<IMatch>();

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();

            var command = new MySqlCommand("SELECT * FROM matches WHERE numberOfSeatsAvailable >= @minSeats ORDER BY numberOfSeatsAvailable DESC", connection);
            command.Parameters.AddWithValue("@minSeats", minSeats);

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    matches.Add(
                        new Match(
                            reader.GetInt32("matchId"),
                            reader.GetInt32("teamAId"),
                            reader.GetInt32("teamBId"),
                            reader.GetInt32("stadiumId"),
                            reader.GetInt32("numberOfSeatsAvailable"),
                            reader.GetFloat("ticketPrice")
                        )
                    );
                }
            }
        }
        return matches;
    }

    public IMatch Add(IMatch match){
        logger.Info($"Adding match: {match.matchId}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO matches (matchId, teamAId, teamBId, stadiumId, numberOfSeatsAvailable, ticketPrice) VALUES (@matchId, @teamAId, @teamBId, @stadiumId, @numberOfSeatsAvailable, @ticketPrice)", connection);
            command.Parameters.AddWithValue("@matchId", match.matchId);
            command.Parameters.AddWithValue("@teamAId", match.teamAId);
            command.Parameters.AddWithValue("@teamBId", match.teamBId);
            command.Parameters.AddWithValue("@stadiumId", match.stadiumId);
            command.Parameters.AddWithValue("@numberOfSeatsAvailable", match.numberOfSeatsAvailable);
            command.Parameters.AddWithValue("@ticketPrice", match.ticketPrice);

            command.ExecuteNonQuery();
        }
        return match; 
    }
    public IMatch FindById(int id) {
        logger.Info("Fetching match with id: {id}");
        var matches = new List<IMatch>();

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM matches WHERE matchId = @matchId", connection);

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    return new Match(
                        reader.GetInt32("matchId"),
                        reader.GetInt32("teamAId"),
                        reader.GetInt32("teamBId"),
                        reader.GetInt32("stadiumId"),
                        reader.GetInt32("numberOfSeatsAvailable"),
                        reader.GetFloat("ticketPrice")
                    );
                    
                }
            }
        }
        return null;
    }
    public List<IMatch> FindAll() {
        /* SELECT * FROM Matches... */
        logger.Info("Fetching all matches.");
        var matches = new List<IMatch>();

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();

            var command = new MySqlCommand("SELECT * FROM matches" , connection);

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    matches.Add(
                        new Match(
                            reader.GetInt32("matchId"),
                            reader.GetInt32("teamAId"),
                            reader.GetInt32("teamBId"),
                            reader.GetInt32("stadiumId"),
                            reader.GetInt32("numberOfSeatsAvailable"),
                            reader.GetFloat("ticketPrice")
                        )
                    );
                }
            }
        }
        return matches;
    }

    public IMatch Update(IMatch match) {
        //i do not know if i should make it update all the fields or just the number of seats available, but for the moment i will leave it as is 
        logger.Info($"Updating match {match.matchId}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            
            var command = new MySqlCommand("UPDATE Matches SET numberOfSeatsAvailable = @seats WHERE matchId = @id", connection);
            command.Parameters.AddWithValue("@seats", match.numberOfSeatsAvailable);
            command.Parameters.AddWithValue("@id", match.matchId);
            command.ExecuteNonQuery();
        }
        return match;
    }

    public IMatch Delete(int id) {
        logger.Info($"Attempting to delete match ID: {id}");

        IMatch deletedMatch = FindById(id);
        if (deletedMatch == null) return null;

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Matches WHERE matchId = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            try {
                command.ExecuteNonQuery();
                logger.Info("Match deleted successfully.");
                return deletedMatch;
            }
            catch (MySqlException ex) {
                // code 1451 is MySQL code for a FK constraint violation
                if (ex.Number == 1451) {
                    logger.Error($"Cannot delete Match {id} because tickets have already been sold for it.");
                    // useful for UI to throw this exception
                    throw new Exception("Cannot delete this match. Tickets exist.");
                }
                throw; // rethrow if it's a different database error
            }
        }
    }
}