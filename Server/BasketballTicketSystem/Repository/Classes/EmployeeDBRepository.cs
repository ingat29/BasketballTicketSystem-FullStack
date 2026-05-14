using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

public class EmployeeDBRepository : IEmployeeRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public EmployeeDBRepository() { logger.Info("Initializing EF EmployeeDBRepository"); }

    public IEmployee FindByUsernameAndPassword(string username, string password) {
        logger.Info($"Finding employee with username: {username}");
        using (var context = new BasketballContext()) {
            return context.Employees.FirstOrDefault(e => e.username == username && e.password == password);
        }
    }

    public IEmployee Add(IEmployee employee) {
        logger.Info($"Adding employee: {employee.username}");
        using (var context = new BasketballContext()) {
            context.Employees.Add((Employee)employee);
            context.SaveChanges();
        }
        return employee;
    }

    public IEmployee FindById(string id) {
        using (var context = new BasketballContext()) {
            return context.Employees.Find(id); // .Find() automatically looks for the [Key]
        }
    }

    public List<IEmployee> FindAll() {
        using (var context = new BasketballContext()) {
            return context.Employees.Cast<IEmployee>().ToList();
        }
    }

    public IEmployee Update(IEmployee employee) {
        using (var context = new BasketballContext()) {
            context.Employees.Update((Employee)employee);
            context.SaveChanges();
        }
        return employee;
    }

    public IEmployee Delete(string id) {
        using (var context = new BasketballContext()) {
            var emp = context.Employees.Find(id);
            if (emp != null) {
                context.Employees.Remove(emp);
                context.SaveChanges();
            }
            return emp;
        }
    }
}