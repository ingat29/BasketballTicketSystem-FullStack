package Model;

public class Customer extends Entity<Integer>{
    private String fullName;

    public Customer(Integer customerId, String fullName) {
        super(customerId);
        this.fullName = fullName;
    }

    public String getFullName(){return this.fullName;}
    public void setFullName(String fullName){this.fullName = fullName;}
}
