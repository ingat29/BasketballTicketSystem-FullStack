package Networking;

import Model.Match;
import javafx.scene.control.TableView;

public class TicketObserver implements ITicketObserver {
    TableView<Match> matchTable;

    public TicketObserver(TableView<Match> matchTable){
        this.matchTable= matchTable;
    }

    @Override
    public void matchUpdated(Match updatedMatch) {
        javafx.application.Platform.runLater(() -> {
            System.out.println("Live update received! Match " + updatedMatch.getId() + " now has " + updatedMatch.getAvailableSeats() + " seats.");

            for (int i = 0; i < matchTable.getItems().size(); i++) {
                if (matchTable.getItems().get(i).getId().equals(updatedMatch.getId())) {
                    matchTable.getItems().set(i, updatedMatch);
                    break;
                }
            }
        });
    }
}
