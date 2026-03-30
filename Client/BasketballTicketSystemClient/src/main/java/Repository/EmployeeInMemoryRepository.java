package Repository;
import Model.Employee;
import Model.EmployeeId;
import Repository.Interfaces.IEmployeeRepository;

public class EmployeeInMemoryRepository extends InMemoryRepository<EmployeeId, Employee> implements IEmployeeRepository {
    @Override
    public Employee findByUsernameAndPassword(String username, String password) {
        return entities.values().stream()
                .filter(e -> e.getUsername().equals(username) && e.getPassword().equals(password))
                .findFirst()
                .orElse(null);
    }
}