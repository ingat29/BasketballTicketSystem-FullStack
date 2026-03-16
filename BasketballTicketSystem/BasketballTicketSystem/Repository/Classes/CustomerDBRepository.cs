using System;
using System.Collections.Generic;
using MySqlConnector;
using NLog;

public class CustomerDBRepository : ICustomerRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public CustomerDBRepository() { logger.Info("Initializing CustomerDBRepository"); }

    public ICustomer FindByName(string name) {
        logger.Info($"Finding customer by name: {name}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Customers WHERE fullName = @name", connection);
            command.Parameters.AddWithValue("@name", name);

            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    return new Customer(reader.GetInt32("customerId"), reader.GetString("fullName"));
                }
            }
        }
        return null;
    }

    public ICustomer Add(ICustomer customer) {
        logger.Info($"Adding customer: {customer.fullName}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Customers (fullName) VALUES (@name)", connection);
            command.Parameters.AddWithValue("@name", customer.fullName);
            command.ExecuteNonQuery();
        }
        return customer;
    }

    public ICustomer FindById(int id) {
        logger.Info($"Finding customer by ID: {id}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Customers WHERE customerId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) return new Customer(reader.GetInt32("customerId"), reader.GetString("fullName"));
            }
        }
        return null;
    }

    public List<ICustomer> FindAll() {
        logger.Info("Finding all customers");
        var list = new List<ICustomer>();
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Customers", connection);
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) list.Add(new Customer(reader.GetInt32("customerId"), reader.GetString("fullName")));
            }
        }
        return list;
    }

    public ICustomer Update(ICustomer entity) {
        Customer customer = (Customer)entity;
        logger.Info($"Updating customer ID: {customer.customerId}");
        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("UPDATE Customers SET fullName = @name WHERE customerId = @id", connection);
            command.Parameters.AddWithValue("@name", customer.fullName);
            command.Parameters.AddWithValue("@id", customer.customerId);
            command.ExecuteNonQuery();
        }
        return entity;
    }

    public ICustomer Delete(int id) {
        logger.Info($"Deleting customer ID: {id}");
        ICustomer deletedCustomer = FindById(id);
        if (deletedCustomer == null) return null;

        using (var connection = DatabaseUtils.GetConnection()) {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Customers WHERE customerId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        return deletedCustomer;
    }
}