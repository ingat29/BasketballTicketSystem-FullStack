using System;

public interface ICustomer
{
	int matchId { get; set; }
	ITeam teamA { get; set; }
	ITeam teamB { get; set; }
	IStadium stadium { get; set; }
	float ticketPrice { get; set; }
}
