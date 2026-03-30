package Model;

import java.util.Objects;

public class EmployeeId {
    private String username;
    private String password;

    public EmployeeId(String username, String password) {
        this.username = username;
        this.password = password;
    }

    public void setUsername(String username) { this.username = username; }
    public void setPassword(String password) { this.password = password; }

    public String getUsername() { return username; }
    public String getPassword() { return password; }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        EmployeeId that = (EmployeeId) o;
        return Objects.equals(username, that.username) && Objects.equals(password, that.password);
    }

    @Override
    public int hashCode() {
        return Objects.hash(username, password);
    }
}