package Repository;
import Model.Team;
import Repository.Interfaces.ITeamRepository;

public class TeamInMemoryRepository extends InMemoryRepository<Integer, Team> implements ITeamRepository {

    public TeamInMemoryRepository() {
        String[] teamNames = {"Lakers", "Bulls", "Celtics", "Warriors", "Nets", "Heat", "Knicks", "Spurs", "Mavericks", "Suns", "Bucks", "76ers"};
        for (int i = 0; i < teamNames.length; i++) {
            this.add(new Model.Team(i + 1, teamNames[i]));
        }
    }

}