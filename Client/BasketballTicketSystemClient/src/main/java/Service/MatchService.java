package Service;

import Model.Match;
import Repository.Interfaces.IMatchRepository;
import java.util.List;

public class MatchService {
    private IMatchRepository matchRepository;

    public MatchService(IMatchRepository matchRepository) {
        this.matchRepository = matchRepository;
    }

    public List<Match> getAllAvailableMatches() {
        return matchRepository.findAllAvailableMatchesOrderedDescending();
    }

    public List<Match> getAvailableMatches(int minSeats) {
        return matchRepository.findAvailableMatchesOrderedDescending(minSeats);
    }
}