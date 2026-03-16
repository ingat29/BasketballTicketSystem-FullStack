using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class TeamDBRepository : ITeamRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ITeam Add(ITeam team) {       
        logger.Info($"Adding team: {team.name}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Teams (name) VALUES (@name)", connection);
            command.Parameters.AddWithValue("@name", team.name);
            command.ExecuteNonQuery();
        }
        return team;
    }

    public ITeam FindById(int id) {
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Teams WHERE teamId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) return new Team(reader.GetInt32("teamId"), reader.GetString("name"));
            }
        }
        return null;
    }

    public List<ITeam> FindAll() {
        var list = new List<ITeam>();
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Teams", connection);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) list.Add(new Team(reader.GetInt32("teamId"), reader.GetString("name")));
            }
        }
        return list;
    }

    public ITeam Update(ITeam team) {
        //Team team = (Team)entity;
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("UPDATE Teams SET name = @name WHERE teamId = @id", connection);
            command.Parameters.AddWithValue("@name", team.name);
            command.Parameters.AddWithValue("@id", team.teamId);
            command.ExecuteNonQuery();
        }
        return team;
    }

    public ITeam Delete(int id) {
        ITeam deletedTeam = FindById(id);
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Teams WHERE teamId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        return deletedTeam;
    }
}