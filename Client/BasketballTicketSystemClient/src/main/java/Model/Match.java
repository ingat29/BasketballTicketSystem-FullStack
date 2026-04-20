package Model;

import java.time.LocalDateTime;

public class Match extends Entity<Integer> {
    private Team teamA;
    private Team teamB;
    private Stadium stadium;
    private LocalDateTime matchDate;
    private float ticketPrice;
    private int availableSeats;

    public Match(Integer id, Team teamA, Team teamB, Stadium stadium /* ,LocalDateTime matchDate */, float ticketPrice, int availableSeats) {
        super(id);
        this.teamA = teamA;
        this.teamB = teamB;
        this.stadium = stadium;
//        this.matchDate = matchDate;
        this.ticketPrice = ticketPrice;
        this.availableSeats = availableSeats;
    }

    // Getters and Setters
    public Team getTeamA() { return teamA; }
    public void setTeamA(Team teamA) { this.teamA = teamA; }

    public Team getTeamB() { return teamB; }
    public void setTeamB(Team teamB) { this.teamB = teamB; }

    public Stadium getStadium() { return stadium; }
    public void setStadium(Stadium stadium) { this.stadium = stadium; }

//    public LocalDateTime getMatchDate() { return matchDate; }
//    public void setMatchDate(LocalDateTime matchDate) { this.matchDate = matchDate; }

    public float getTicketPrice() { return ticketPrice; }
    public void setTicketPrice(float ticketPrice) { this.ticketPrice = ticketPrice; }

    public int getAvailableSeats() { return availableSeats; }
    public void setAvailableSeats(int availableSeats) { this.availableSeats = availableSeats; }
}
