using BasketballTicketSystem.Networking.Protobuf;
using Google.Protobuf;
using System;
using System.Net;
using System.Net.Sockets;

class TestProgram {

    //A "Hello world" of the protobuf world 
    public class SocketTest {
        public static void RunServer() {
            TcpListener listener = new TcpListener(IPAddress.Any, 5555);
            listener.Start();
            Console.WriteLine("Server started on port 5555. Waiting for a client...");

            // Pauses here until a client connects!
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("A Java client connected!");
            NetworkStream stream = client.GetStream();

            // Wait for the client to send a Request
            Request incomingRequest = Request.Parser.ParseDelimitedFrom(stream);

            Console.WriteLine($"The client sent a: {incomingRequest.PayloadCase}");
            if (incomingRequest.PayloadCase == Request.PayloadOneofCase.Login) {
                Console.WriteLine($"Username: {incomingRequest.Login.Username}");
            }

            // Send a Response back
            Response response = new Response {
                Login = new LoginResponse {
                    Success = true,
                    ErrorMessage = "Hello from the C# Server!"
                }
            };

            response.WriteDelimitedTo(stream);
            Console.WriteLine("Response sent back to client.");
        }
    }

    //static void Main(string[] args) {
    //    Console.WriteLine("Starting Database Test");

    //    try {
    //        IEmployeeRepository employeeRepo = new EmployeeDBRepository();

    //        var employees = employeeRepo.FindAll();

    //        Console.WriteLine($"Success! Connected to the database and found {employees.Count} employees.");
    //        Console.WriteLine("Check bin/Debug folder for the application.log file to see the NLog output.");
        
    //        SocketTest.RunServer();
    //    }
    //    catch (Exception ex) {
    //        Console.WriteLine("Something went wrong:");
    //        Console.WriteLine(ex.Message);
    //    }

    //    Console.ReadLine();
    //}
}