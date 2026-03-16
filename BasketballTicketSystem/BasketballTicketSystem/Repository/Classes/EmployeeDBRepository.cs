using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class EmployeeDBRepository : IEmployeeRepository
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public EmployeeDBRepository()
    {
        logger.Info("Initializing EmployeeDBRepository");
    }

    public IEmployee FindByUsernameAndPassword(string username, string password)
    {
        logger.Info($"Finding employee with username: {username}");
        using (var connection = DatabaseUtils.GetConnection())
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM employees WHERE username = @user AND password = @pass", connection);
            command.Parameters.AddWithValue("@user", username);
            command.Parameters.AddWithValue("@pass", password);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Employee(reader.GetString("username"), reader.GetString("password"));
                }
            }
        }
        return null;
    }

    public IEmployee Add(IEmployee employee)
    {
        logger.Info($"Adding employee: {employee.username}");
        using (var connection = DatabaseUtils.GetConnection())
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO employees (username, password) VALUES (@user, @pass)", connection);
            command.Parameters.AddWithValue("@user", employee.username);
            command.Parameters.AddWithValue("@pass", employee.password);
            command.ExecuteNonQuery();
        }
        return employee;
    }

    public IEmployee FindById(string id)//Id being username in this case
    {
        logger.Info($"Finding employee by ID (username): {id}");
        using (var connection = DatabaseUtils.GetConnection())
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM employees WHERE username = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Employee(reader.GetString("username"), reader.GetString("password"));
                }
            }
        }
        return null;
    }

    public List<IEmployee> FindAll() {
        
        logger.Info($"Finding all employees");
        List<IEmployee> employees = new List<IEmployee>();
        using (var connection = DatabaseUtils.GetConnection()){
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM employees", connection);

            using (var reader = command.ExecuteReader()){
                while (reader.Read()){
                    var employee= new Employee(reader.GetString("username"), reader.GetString("password"));
                    employees.Add(employee);
                }
            }
        }
        return employees; 
    }
    public IEmployee Update(IEmployee employee) {
        logger.Info($"Updating employee: {employee.username}");

        using (var connection = DatabaseUtils.GetConnection()){
            connection.Open();
            var command = new MySqlCommand("UPDATE employees SET password=@pass WHERE username=@user", connection);
            command.Parameters.AddWithValue("@user", employee.username);
            command.Parameters.AddWithValue("@pass", employee.password);
            command.ExecuteNonQuery();
        }
        return employee; 
    }
    public IEmployee Delete(string id){
        logger.Info($"Deleting employee with ID (username): {id}");
        using (var connection = DatabaseUtils.GetConnection()){
            connection.Open();
            var command = new MySqlCommand("DELETE FROM employees WHERE username=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        return null; 
    }
}