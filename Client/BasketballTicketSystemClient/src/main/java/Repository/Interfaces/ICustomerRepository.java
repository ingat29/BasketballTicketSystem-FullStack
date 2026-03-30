package Repository.Interfaces;
import Model.Customer;

public interface ICustomerRepository extends IRepository<Integer, Customer> {
    Customer findByName(String name);
}