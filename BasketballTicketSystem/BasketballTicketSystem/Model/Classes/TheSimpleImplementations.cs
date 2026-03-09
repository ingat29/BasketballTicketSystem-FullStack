using System;

public class Stadium : IStadium
{
    public int stadiumId { get; set; }
    public string name { get; set; }
    public int capacity { get; set; }

    public Stadium() { }

    public Stadium(int stadiumId, string name, int capacity)
    {
        this.stadiumId = stadiumId;
        this.name = name;
        this.capacity = capacity;
    }
}

public class Employee : IEmployee
{
    public string username { get; set; }
    public string password { get; set; }

    public Employee() { }

    public Employee(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class Ticket : ITicket
{
    public int ticketId { get; set; }
    public int matchId { get; set; }
    public int customerId{ get; set; }
    public int numberOfSeats { get; set; }

    public Ticket() { }

    public Ticket(int ticketId, int matchId, int customerId, int numberOfSeats)
    {
        this.ticketId = ticketId;
        this.matchId = matchId;
        this.customerId = customerId;
        this.numberOfSeats = numberOfSeats;
    }
}

public class Customer : ICustomer
{
    public int customerId { get; set; }
    public string fullName { get; set; }

    public Customer() { }

    public Customer(int customerId, string fullName)
    {
        this.customerId = customerId;
        this.fullName = fullName;
    }
}

public class Match : IMatch
{
    public int matchId { get; set; }
    public int teamAId { get; set; }
    public int teamBId { get; set; }
    public int stadiumId { get; set; }
    public int numberOfSeatsAvailable { get; set; }
    public float ticketPrice { get; set; }

    public Match() { }

    public Match(int matchId, int teamAId, int teamBId, int stadiumId, int numberOfSeatsAvailable, float ticketPrice)
    {
        this.matchId = matchId;
        this.teamAId = teamAId;
        this.teamBId = teamBId;
        this.stadiumId = stadiumId;
        this.numberOfSeatsAvailable = numberOfSeatsAvailable;
        this.ticketPrice = ticketPrice;
    }
}

public class Team : ITeam
{
    public int teamId { get; set; }
    public string name { get; set; }

    public Team() { }

    public Team(int teamId, string name)
    {
        this.teamId = teamId;
        this.name = name;
    }
}