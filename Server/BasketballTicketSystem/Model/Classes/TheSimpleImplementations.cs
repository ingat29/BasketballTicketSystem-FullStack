using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Stadiums")]
public class Stadium : IStadium {
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Tells EF Core the DB handles auto-increment
    public int stadiumId { get; set; }
    public string name { get; set; }
    public int capacity { get; set; }

    public Stadium() { }

    public Stadium(int stadiumId, string name, int capacity) {
        this.stadiumId = stadiumId;
        this.name = name;
        this.capacity = capacity;
    }
}

[Table("employees")]
public class Employee : IEmployee {
    [Key] // Since this isn't an integer named "Id", we MUST tell EF Core this is the primary key
    public string username { get; set; }
    public string password { get; set; }

    public Employee() { }

    public Employee(string username, string password) {
        this.username = username;
        this.password = password;
    }
}

[Table("Tickets")]
public class Ticket : ITicket {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ticketId { get; set; }
    public int matchId { get; set; }
    public int customerId { get; set; }
    public int numberOfSeats { get; set; }

    public Ticket() { }

    public Ticket(int ticketId, int matchId, int customerId, int numberOfSeats) {
        this.ticketId = ticketId;
        this.matchId = matchId;
        this.customerId = customerId;
        this.numberOfSeats = numberOfSeats;
    }
}

[Table("Customers")]
public class Customer : ICustomer {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int customerId { get; set; }
    public string fullName { get; set; }

    public Customer() { }

    public Customer(int customerId, string fullName) {
        this.customerId = customerId;
        this.fullName = fullName;
    }
}

[Table("matches")]
public class Match : IMatch {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int matchId { get; set; }
    public int teamAId { get; set; }
    public int teamBId { get; set; }
    public int stadiumId { get; set; }
    public int numberOfSeatsAvailable { get; set; }
    public float ticketPrice { get; set; }

    public Match() { }

    public Match(int matchId, int teamAId, int teamBId, int stadiumId, int numberOfSeatsAvailable, float ticketPrice) {
        this.matchId = matchId;
        this.teamAId = teamAId;
        this.teamBId = teamBId;
        this.stadiumId = stadiumId;
        this.numberOfSeatsAvailable = numberOfSeatsAvailable;
        this.ticketPrice = ticketPrice;
    }
}

[Table("Teams")]
public class Team : ITeam {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int teamId { get; set; }
    public string name { get; set; }

    public Team() { }

    public Team(int teamId, string name) {
        this.teamId = teamId;
        this.name = name;
    }
}