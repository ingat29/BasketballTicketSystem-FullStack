package Repository;
import Model.Customer;
import Repository.Interfaces.ICustomerRepository;

public class CustomerInMemoryRepository extends InMemoryRepository<Integer, Customer> implements ICustomerRepository {
    @Override
    public Customer findByName(String name) {
        return entities.values().stream()
                .filter(c -> c.getFullName().equals(name))
                .findFirst()
                .orElse(null);
    }
}