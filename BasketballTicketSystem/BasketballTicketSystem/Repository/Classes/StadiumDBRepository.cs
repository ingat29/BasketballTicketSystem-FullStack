using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class StadiumDBRepository : IStadiumRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public IStadium Add(IStadium stadium) {
        //Stadium stadium = (Stadium)entity;
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Stadiums (name, capacity) VALUES (@name, @capacity)", connection);
            command.Parameters.AddWithValue("@name", stadium.name);
            command.Parameters.AddWithValue("@capacity", stadium.capacity);
            command.ExecuteNonQuery();
        }
        return stadium;
    }

    public IStadium FindById(int id) {
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Stadiums WHERE stadiumId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) return new Stadium(reader.GetInt32("stadiumId"), reader.GetString("name"), reader.GetInt32("capacity"));
            }
        }
        return null;
    }

    public List<IStadium> FindAll() {
        var list = new List<IStadium>();
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Stadiums", connection);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) list.Add(new Stadium(reader.GetInt32("stadiumId"), reader.GetString("name"), reader.GetInt32("capacity")));
            }
        }
        return list;
    }

    public IStadium Update(IStadium stadium) {
        //Stadium stadium = (Stadium)entity;
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("UPDATE Stadiums SET name = @name, capacity = @capacity WHERE stadiumId = @id", connection);
            command.Parameters.AddWithValue("@name", stadium.name);
            command.Parameters.AddWithValue("@capacity", stadium.capacity);
            command.Parameters.AddWithValue("@id", stadium.stadiumId);
            command.ExecuteNonQuery();
        }
        return stadium;
    }

    public IStadium Delete(int id) {
        IStadium deletedStadium = FindById(id);
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Stadiums WHERE stadiumId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        return deletedStadium;
    }
}