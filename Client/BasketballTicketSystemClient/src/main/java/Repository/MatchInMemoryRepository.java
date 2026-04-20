package Repository;
import Model.Match;
import Repository.Interfaces.IMatchRepository;

import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class MatchInMemoryRepository extends InMemoryRepository<Integer, Match> implements IMatchRepository {

    public MatchInMemoryRepository() {
        Model.Team t1 = new Model.Team(1, "Lakers");
        Model.Team t2 = new Model.Team(2, "Bulls");
        Model.Team t3 = new Model.Team(3, "Celtics");
        Model.Team t4 = new Model.Team(4, "Warriors");
        Model.Team t5 = new Model.Team(5, "Nets");
        Model.Team t6 = new Model.Team(6, "Heat");

        Model.Stadium s1 = new Model.Stadium(1, "Crypto.com Arena", 20000);
        Model.Stadium s2 = new Model.Stadium(2, "United Center", 21000);
        Model.Stadium s3 = new Model.Stadium(3, "TD Garden", 19500);

        java.time.LocalDateTime date = java.time.LocalDateTime.now();

        this.add(new Model.Match(1, t1, t2, s1,  50.0f, 20000));
        this.add(new Model.Match(2, t3, t4, s3,  75.5f, 19500));
        this.add(new Model.Match(3, t5, t6, s2,  40.0f, 21000));
        this.add(new Model.Match(4, t1, t3, s1,  100.0f, 500)); // Almost sold out
        this.add(new Model.Match(5, t2, t4, s2,  45.0f, 15000));
        this.add(new Model.Match(6, t5, t1, s3,  60.0f, 100)); // Almost sold out
        this.add(new Model.Match(7, t6, t2, s1,  35.0f, 20000));
        this.add(new Model.Match(8, t4, t5, s2,  55.0f, 0));   // Sold out
        this.add(new Model.Match(9, t3, t6, s3,  80.0f, 5000));
        this.add(new Model.Match(10, t1, t4, s1, 120.0f, 10)); // VIP match
    }

    @Override
    public List<Match> findAllAvailableMatchesOrderedDescending() {
        return entities.values().stream()
                .filter(m -> m.getAvailableSeats() > 0)
                .sorted(Comparator.comparing(Match::getAvailableSeats).reversed())
                .collect(Collectors.toList());
    }

    @Override
    public List<Match> findAvailableMatchesOrderedDescending(int minSeats) {
        return entities.values().stream()
                .filter(m -> m.getAvailableSeats() >= minSeats)
                .sorted(Comparator.comparing(Match::getAvailableSeats).reversed())
                .collect(Collectors.toList());
    }
}