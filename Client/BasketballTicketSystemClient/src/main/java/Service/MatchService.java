package Service;

import Model.Match;
import Networking.ServerProxy;
import java.util.List;

public class MatchService {
    private ServerProxy proxy;

    public MatchService(ServerProxy proxy) {
        this.proxy = proxy;
    }

    public List<Match> getAllAvailableMatches() {
        try {
            return proxy.getAvailableMatches(0);
        } catch (Exception e) {
            System.err.println("Failed to fetch matches: " + e.getMessage());
            return null;
        }
    }

    public List<Match> getAvailableMatches(int minSeats) {
        try {
            return proxy.getAvailableMatches(minSeats);
        } catch (Exception e) {
            System.err.println("Failed to fetch matches: " + e.getMessage());
            return null;
        }
    }
}