using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using BasketballTicketSystem.Networking.Protobuf;

public class ServerApp {
    private TcpListener _listener;
    private bool _isRunning;

    private IEmployeeRepository _employeeRepo;
    private IMatchRepository _matchRepo;
    private ITicketRepository _ticketRepo;
    private ICustomerRepository _customerRepo;

    // Here i will eventually pass the Repositories/Services into this class
    public ServerApp(IEmployeeRepository employeeRepo, IMatchRepository matchRepo, ITicketRepository ticketRepo, ICustomerRepository customerRepo) {
        _employeeRepo = employeeRepo;
        _matchRepo = matchRepo;
        _ticketRepo = ticketRepo;
        _customerRepo = customerRepo;

        _listener = new TcpListener(IPAddress.Any, 5555);
    }

    public void Start() {
        _listener.Start();
        _isRunning = true;
        Console.WriteLine("Server started on port 5555. Waiting for clients...");

        while (_isRunning) {
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine($"New client connected: {client.Client.RemoteEndPoint}");

            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    public void Stop() {
        _isRunning = false;

        _listener.Stop();

        Console.WriteLine("Server is shutting down...");
    }

    private void HandleClient(TcpClient client) {
        NetworkStream stream = client.GetStream();

        try {
            // Keep listening to this client until they disconnect
            while (true) {
                // This line will pause and wait until the client sends a message
                Request req = Request.Parser.ParseDelimitedFrom(stream);

                // If ParseDelimitedFrom returns null, the client disconnected gracefully
                if (req == null) break;

                // Figure out what the client wants and send a response
                Response response = ProcessRequest(req);

                // Send the answer back
                if (response != null) {
                    response.WriteDelimitedTo(stream);
                }
            }
        }
        catch (Exception ex) {
            // If the client forcefully closes the app, it throws an exception here
            Console.WriteLine($"Client disconnected abruptly: {ex.Message}");
        }
        finally {
            client.Close();
            Console.WriteLine("Client connection closed.");
        }
    }

    private Response ProcessRequest(Request req) {
        switch (req.PayloadCase) {
            case Request.PayloadOneofCase.Login:
                return HandleLogin(req.Login);

            case Request.PayloadOneofCase.GetMatches:
                return HandleGetMatches(req.GetMatches);

            case Request.PayloadOneofCase.BuyTicket:
                return HandleBuyTicket(req.BuyTicket);

            default:
                Console.WriteLine("Received an unknown request type.");
                return null;
        }
    }

    private Response HandleLogin(LoginRequest loginReq) {
        Console.WriteLine($"User attempting to login: {loginReq.Username}");

        IEmployee employee = _employeeRepo.FindByUsernameAndPassword(loginReq.Username, loginReq.Password);

        bool isValid = employee != null;

        return new Response {
            Login = new LoginResponse {
                Success = isValid,
                ErrorMessage = isValid ? "" : "Invalid username or password."
            }
        };
    }

    private Response HandleGetMatches(GetMatchesRequest req) {
        List<IMatch> dbMatches;
        if (req.MinSeats > 0) {
            dbMatches = _matchRepo.FindAvailableMatchesOrderedDescending(req.MinSeats);
        }
        else {
            dbMatches = _matchRepo.FindAllAvailableMatchesOrderedDescending();
        }

        GetMatchesResponse responseData = new GetMatchesResponse();
        
        // Translate from C# DB Model to Protobuf DTO
        foreach (IMatch dbMatch in dbMatches) {
            MatchDto dto = new MatchDto {
                MatchId = dbMatch.matchId,
                TeamAId = dbMatch.teamAId,
                TeamBId = dbMatch.teamBId,
                StadiumId = dbMatch.stadiumId,
                NumberOfSeatsAvailable = dbMatch.numberOfSeatsAvailable,
                TicketPrice = dbMatch.ticketPrice
            };
            
            responseData.Matches.Add(dto);
        }

        return new Response { GetMatches = responseData };
    }

    private Response HandleBuyTicket(BuyTicketRequest req) {
        IMatch match = _matchRepo.FindById(req.MatchId);

        if (match == null || match.numberOfSeatsAvailable < req.NumberOfSeats) {
            return new Response {
                BuyTicket = new BuyTicketResponse {
                    Success = false,
                    ErrorMessage = "Not enough available seats or invalid match!"
                }
            };
        }

        match.numberOfSeatsAvailable -= req.NumberOfSeats;
        _matchRepo.Update(match);

        Ticket newTicket = new Ticket {
            matchId = req.MatchId,
            customerId = req.CustomerId,
            numberOfSeats = req.NumberOfSeats
        };

        ITicket savedTicket = _ticketRepo.Add(newTicket);

        return new Response {
            BuyTicket = new BuyTicketResponse {
                Success = true,
                ErrorMessage = "",
                PurchasedTicket = new TicketDto {
                    TicketId = savedTicket.ticketId,
                    MatchId = savedTicket.matchId,
                    CustomerId = savedTicket.customerId,
                    NumberOfSeats = savedTicket.numberOfSeats
                }
            }
        };
    }
}