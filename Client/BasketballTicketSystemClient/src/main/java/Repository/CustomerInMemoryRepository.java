package Repository;
import Model.Customer;
import Repository.Interfaces.ICustomerRepository;

public class CustomerInMemoryRepository extends InMemoryRepository<Integer, Customer> implements ICustomerRepository {
    public CustomerInMemoryRepository() {
        for (int i = 1; i <= 20; i++) {
            this.add(new Model.Customer(i, "Customer Name " + i));
        }
    }

    @Override
    public Customer findByName(String name) {
        return entities.values().stream()
                .filter(c -> c.getFullName().equals(name))
                .findFirst()
                .orElse(null);
    }

    @Override
    public Customer findById(Integer id){
        return entities.values().stream()
                .filter(c -> c.getId().equals(id))
                .findFirst()
                .orElse(null);
    }
}