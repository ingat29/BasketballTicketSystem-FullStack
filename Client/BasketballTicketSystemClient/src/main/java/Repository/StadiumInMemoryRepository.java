package Repository;
import Model.Stadium;
import Repository.Interfaces.IStadiumRepository;

public class StadiumInMemoryRepository extends InMemoryRepository<Integer, Stadium> implements IStadiumRepository {}