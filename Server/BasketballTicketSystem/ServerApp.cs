// Commented out, since the code is now deprecated by using REST 

//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using Google.Protobuf;
//using BasketballTicketSystem.Networking.Protobuf;

//public class ServerApp {
//    private TcpListener _listener;
//    private bool _isRunning;

//    private readonly object _clientsLock = new object();
//    private List<NetworkStream> _activeClients = new List<NetworkStream>();

//    private IEmployeeRepository _employeeRepo;
//    private IMatchRepository _matchRepo;
//    private ITicketRepository _ticketRepo;
//    private ICustomerRepository _customerRepo;
//    private ITeamRepository _teamRepo;
//    private IStadiumRepository _stadiumRepo;

//    public ServerApp(IEmployeeRepository empRepo, IMatchRepository matchRepo,
//                     ITicketRepository ticketRepo, ICustomerRepository custRepo,
//                     ITeamRepository teamRepo, IStadiumRepository stadiumRepo) {
//        _employeeRepo = empRepo;
//        _matchRepo = matchRepo;
//        _ticketRepo = ticketRepo;
//        _customerRepo = custRepo;
//        _teamRepo = teamRepo;
//        _stadiumRepo = stadiumRepo;

//        _listener = new TcpListener(IPAddress.Any, 5555);
//    }

//    private MatchDto BuildMatchDto(IMatch dbMatch) {

//        ITeam teamA = _teamRepo.FindById(dbMatch.teamAId);
//        ITeam teamB = _teamRepo.FindById(dbMatch.teamBId);
//        IStadium stadium = _stadiumRepo.FindById(dbMatch.stadiumId);

//        return new MatchDto {
//            MatchId = dbMatch.matchId,
//            TeamA = new TeamDto { TeamId = teamA.teamId, Name = teamA.name },
//            TeamB = new TeamDto { TeamId = teamB.teamId, Name = teamB.name },
//            Stadium = new StadiumDto { StadiumId = stadium.stadiumId, Name = stadium.name, Capacity = stadium.capacity },
//            NumberOfSeatsAvailable = dbMatch.numberOfSeatsAvailable,
//            TicketPrice = dbMatch.ticketPrice
//        };
//    }

//    public void Start() {
//        _listener.Start();
//        _isRunning = true;
//        Console.WriteLine("Server started on port 5555. Waiting for clients...");

//        while (_isRunning) {
//            TcpClient client = _listener.AcceptTcpClient();
//            Console.WriteLine($"New client connected: {client.Client.RemoteEndPoint}");

//            Thread clientThread = new Thread(() => HandleClient(client));
//            clientThread.Start();
//        }
//    }

//    public void Stop() {
//        _isRunning = false;

//        _listener.Stop();

//        Console.WriteLine("Server is shutting down...");
//    }

//    private void HandleClient(TcpClient client) {
//        NetworkStream stream = client.GetStream();

//        lock (_clientsLock) {
//            _activeClients.Add(stream);
//        }

//        try {
//            while (true) {
//                Request req = Request.Parser.ParseDelimitedFrom(stream);

//                if (req == null) break;

//                Response response = ProcessRequest(req);

//                if (response != null) {
//                    response.WriteDelimitedTo(stream);
//                }
//            }
//        }
//        catch (Exception ex) {
//            Console.WriteLine($"Client disconnected abruptly: {ex.Message}");
//        }
//        finally {

//            lock (_clientsLock) { 
//                _activeClients.Remove(stream);
//            }

//            client.Close();
//            Console.WriteLine("Client connection closed.");
//        }
//    }

//    private Response ProcessRequest(Request req) {
//        switch (req.PayloadCase) {
//            case Request.PayloadOneofCase.Login:
//                return HandleLogin(req.Login);

//            case Request.PayloadOneofCase.GetMatches:
//                return HandleGetMatches(req.GetMatches);

//            case Request.PayloadOneofCase.BuyTicket:
//                return HandleBuyTicket(req.BuyTicket);

//            default:
//                Console.WriteLine("Received an unknown request type.");
//                return null;
//        }
//    }

//    private Response HandleLogin(LoginRequest loginReq) {
//        Console.WriteLine($"User attempting to login: {loginReq.Username}");

//        IEmployee employee = _employeeRepo.FindByUsernameAndPassword(loginReq.Username, loginReq.Password);

//        bool isValid = employee != null;

//        return new Response {
//            Login = new LoginResponse {
//                Success = isValid,
//                ErrorMessage = isValid ? "" : "Invalid username or password."
//            }
//        };
//    }

//    private Response HandleGetMatches(GetMatchesRequest req) {
//        List<IMatch> dbMatches = req.MinSeats > 0
//            ? _matchRepo.FindAvailableMatchesOrderedDescending(req.MinSeats)
//            : _matchRepo.FindAllAvailableMatchesOrderedDescending();

//        GetMatchesResponse responseData = new GetMatchesResponse();

//        foreach (IMatch dbMatch in dbMatches) {
//            // Use our beautiful new helper method!
//            responseData.Matches.Add(BuildMatchDto(dbMatch));
//        }

//        return new Response { GetMatches = responseData };
//    }

//    private Response HandleBuyTicket(BuyTicketRequest req) {
//        IMatch match = _matchRepo.FindById(req.MatchId);

//        if (match == null || match.numberOfSeatsAvailable < req.NumberOfSeats) {
//            return new Response { BuyTicket = new BuyTicketResponse { Success = false, ErrorMessage = "Not enough seats!" } };
//        }

//        match.numberOfSeatsAvailable -= req.NumberOfSeats;
//        _matchRepo.Update(match);

//        Ticket newTicket = new Ticket {
//            matchId = req.MatchId,
//            customerId = req.CustomerId,
//            numberOfSeats = req.NumberOfSeats
//        };
//        ITicket savedTicket = _ticketRepo.Add(newTicket);

//        ICustomer customer = _customerRepo.FindById(req.CustomerId);

//        //Broadcast the updated match info to all clients so they can update their UI
//        NotifyAllClients(match);

//        return new Response {
//            BuyTicket = new BuyTicketResponse {
//                Success = true,
//                ErrorMessage = "",
//                PurchasedTicket = new TicketDto {
//                    TicketId = savedTicket.ticketId,
//                    Match = BuildMatchDto(match),
//                    Customer = new CustomerDto { CustomerId = customer.customerId, Name = customer.fullName },
//                    NumberOfSeats = savedTicket.numberOfSeats
//                }
//            }
//        };
//    }

//    private void NotifyAllClients(IMatch updatedMatch) {
//        Response updateResponse = new Response {
//            Update = new UpdateNotification {
//                UpdatedMatch = BuildMatchDto(updatedMatch)
//            }
//        };

//        lock (_clientsLock) {
//            // We create a quick copy of the list with .ToList() 
//            // This prevents errors if a client disconnects exactly while we are broadcasting
//            foreach (var stream in _activeClients.ToList()) {
//                try {
//                    updateResponse.WriteDelimitedTo(stream);
//                }
//                catch (Exception ex) {
//                    Console.WriteLine($"Failed to send update to a client: {ex.Message}");
//                }
//            }
//        }
//    }
//}