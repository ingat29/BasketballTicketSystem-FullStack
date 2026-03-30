package Repository;
import Model.Team;
import Repository.Interfaces.ITeamRepository;

public class TeamInMemoryRepository extends InMemoryRepository<Integer, Team> implements ITeamRepository {}