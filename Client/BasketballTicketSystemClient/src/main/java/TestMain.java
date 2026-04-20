import java.net.Socket;
import Networking.Protobuf.BasketballProto.*;

public class TestMain {
    public static void main(String[] args) {
        try {
            System.out.println("Connecting to server...");
            Socket socket = new Socket("127.0.0.1", 5555);
            System.out.println("Connected!");

            Request request = Request.newBuilder()
                    .setLogin(
                            LoginRequest.newBuilder()
                                    .setUsername("ingat29")
                                    .setPassword("myPassword123")
                                    .build()
                    )
                    .build();

            request.writeDelimitedTo(socket.getOutputStream());
            System.out.println("Sent login request to server.");

            Response response = Response.parseDelimitedFrom(socket.getInputStream());

            System.out.println("Server replied! Success: " + response.getLogin().getSuccess());
            System.out.println("Server message: " + response.getLogin().getErrorMessage());

            socket.close();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}