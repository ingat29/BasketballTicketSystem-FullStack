package Repository.Interfaces;
import Model.Employee;
import Model.EmployeeId;

public interface IEmployeeRepository extends IRepository<EmployeeId, Employee> {
    Employee findByUsernameAndPassword(String username, String password);
}