package Repository.Interfaces;
import Model.Match;
import java.util.List;

public interface IMatchRepository extends IRepository<Integer, Match> {
    List<Match> findAllAvailableMatchesOrderedDescending();
    List<Match> findAvailableMatchesOrderedDescending(int minSeats);
}
