package Service;

import Model.Employee;
import Repository.Interfaces.IEmployeeRepository; // Or EmployeeRepository depending on what you named it

public class EmployeeService {
    private IEmployeeRepository employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository) {
        this.employeeRepository = employeeRepository;
    }

    public Employee login(String username, String password) {
        //Returns the Employee if found, or null if credentials are wrong
        return employeeRepository.findByUsernameAndPassword(username, password);
    }
}