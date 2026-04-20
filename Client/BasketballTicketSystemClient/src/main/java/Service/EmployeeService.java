package Service;

import Model.Employee;
import Model.EmployeeId;
import Networking.ServerProxy;

public class EmployeeService {
    private ServerProxy proxy;

    public EmployeeService(ServerProxy proxy) {
        this.proxy = proxy;
    }

    public Employee login(String username, String password) {
        try {
            boolean success = proxy.login(username, password);
            if (success) {
                // Return a dummy employee object just so the controller knows it was successful
                return new Employee(new EmployeeId(username, password), username, password, username);
            }
        } catch (Exception e) {
            System.err.println("Login failed: " + e.getMessage());
        }
        return null;
    }
}