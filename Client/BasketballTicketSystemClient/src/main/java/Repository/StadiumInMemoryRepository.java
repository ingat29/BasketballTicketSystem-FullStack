package Repository;
import Model.Stadium;
import Repository.Interfaces.IStadiumRepository;

public class StadiumInMemoryRepository extends InMemoryRepository<Integer, Stadium> implements IStadiumRepository {

    public StadiumInMemoryRepository() {
        this.add(new Model.Stadium(1, "Crypto.com Arena", 20000));
        this.add(new Model.Stadium(2, "United Center", 21000));
        this.add(new Model.Stadium(3, "TD Garden", 19500));
        this.add(new Model.Stadium(4, "Chase Center", 18000));
        this.add(new Model.Stadium(5, "Madison Square Garden", 19500));
    }

}