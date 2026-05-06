package Networking;

import Model.Match;

public interface ITicketObserver {
    void matchUpdated(Match updatedMatch);
}