package Repository;
import Model.Employee;
import Model.EmployeeId;
import Repository.Interfaces.IEmployeeRepository;

public class EmployeeInMemoryRepository extends InMemoryRepository<EmployeeId, Employee> implements IEmployeeRepository {

    public EmployeeInMemoryRepository() {
        for (int i = 1; i <= 15; i++) {
            Model.EmployeeId id = new Model.EmployeeId("employee" + i, "password" + i);
            this.add(new Model.Employee(id, "employee" + i, "password" + i, "Employee FullName " + i));
        }
    }

    @Override
    public Employee findByUsernameAndPassword(String username, String password) {
        return entities.values().stream()
                .filter(e -> e.getUsername().equals(username) && e.getPassword().equals(password))
                .findFirst()
                .orElse(null);
    }
}