using System;

public interface IMatch
{
	int matchId { get; set; }
	int teamAId { get; set; }
	int teamBId { get; set; }
	int stadiumId { get; set; }
	int numberOfSeatsAvailable { get; set; }
    float ticketPrice { get; set; }
}
