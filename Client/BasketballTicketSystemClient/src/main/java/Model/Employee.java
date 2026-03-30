package Model;


public class Employee extends Entity<EmployeeId> {

    public Employee(EmployeeId id, String username, String password, String fullName) {
        super(id);
    }

    public String getUsername() { return getId().getUsername(); }
    public void setUsername(String username) { getId().setUsername(username); }

    public String getPassword() { return getId().getPassword(); }
    public void setPassword(String password) { getId().setPassword(password); }
}
