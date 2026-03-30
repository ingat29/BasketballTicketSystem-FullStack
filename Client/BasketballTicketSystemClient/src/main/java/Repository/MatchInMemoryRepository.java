package Repository;
import Model.Match;
import Repository.Interfaces.IMatchRepository;

import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class MatchInMemoryRepository extends InMemoryRepository<Integer, Match> implements IMatchRepository {
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