package Networking;

import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;
import Networking.Protobuf.BasketballProto.*;
import Model.Match;
import Model.Team;
import Model.Stadium;
import java.net.Socket;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.List;

public class ServerProxy {
    private String host;
    private int port;
    private Socket connection;
    private InputStream input;
    private OutputStream output;

    private BlockingQueue<Response> responses;
    private ITicketObserver observer;

    public ServerProxy(String host, int port) {
        this.host = host;
        this.port = port;
        this.responses = new LinkedBlockingQueue<>();
    }

    public void setObserver(ITicketObserver observer) {
        this.observer = observer;
    }


    public void connect() throws Exception {
        if (connection == null || connection.isClosed()) {
            connection = new Socket(host, port);
            output = connection.getOutputStream();
            input = connection.getInputStream();
            startReaderThread();

            System.out.println("Connected to C# Server at " + host + ":" + port);
        }
    }

    public void disconnect() {
        try {
            if (connection != null) {
                connection.close();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public boolean login(String username, String password) throws Exception {
        // Pack
        Request request = Request.newBuilder()
                .setLogin(
                        LoginRequest.newBuilder()
                                .setUsername(username)
                                .setPassword(password)
                                .build()
                )
                .build();

        // Send & Wait
        request.writeDelimitedTo(output);
        Response response = responses.take();

        // Unpack
        if (response.getPayloadCase() == Response.PayloadCase.LOGIN) {
            LoginResponse loginResp = response.getLogin();
            if (!loginResp.getSuccess()) {
                throw new Exception(loginResp.getErrorMessage());
            }
            return true;
        } else {
            throw new Exception("Received unexpected response type from server.");
        }
    }

    public List<Match> getAvailableMatches(int minSeats) throws Exception {
        Request request = Request.newBuilder()
                .setGetMatches(
                        GetMatchesRequest.newBuilder()
                                .setMinSeats(minSeats)
                                .build()
                )
                .build();

        request.writeDelimitedTo(output);
        Response response = responses.take();

        if (response.getPayloadCase() == Response.PayloadCase.GET_MATCHES) {
            GetMatchesResponse matchResp = response.getGetMatches();
            List<Match> javaMatches = new ArrayList<>();

            for (MatchDto dto : matchResp.getMatchesList()) {

                Team teamA = new Team(dto.getTeamA().getTeamId(), dto.getTeamA().getName());

                Team teamB = new Team(dto.getTeamB().getTeamId(), dto.getTeamB().getName());

                Stadium stadium = new Stadium(
                        dto.getStadium().getStadiumId(),
                        dto.getStadium().getName(),
                        dto.getStadium().getCapacity()
                );

                Match match = new Match(
                        dto.getMatchId(),
                        teamA,
                        teamB,
                        stadium,
                        dto.getTicketPrice(),
                        dto.getNumberOfSeatsAvailable()
                );

                javaMatches.add(match);
            }
            return javaMatches;

        } else {
            throw new Exception("Received unexpected response type from server.");
        }
    }

    public boolean buyTicket(int matchId, int customerId, int numberOfSeats) throws Exception {
        Request request = Request.newBuilder()
                .setBuyTicket(
                        BuyTicketRequest.newBuilder()
                                .setMatchId(matchId)
                                .setCustomerId(customerId)
                                .setNumberOfSeats(numberOfSeats)
                                .build()
                )
                .build();

        request.writeDelimitedTo(output);
        Response response = responses.take();

        if (response.getPayloadCase() == Response.PayloadCase.BUY_TICKET) {
            BuyTicketResponse buyResp = response.getBuyTicket();
            if (!buyResp.getSuccess()) {
                throw new Exception(buyResp.getErrorMessage());
            }
            return true;
        } else {
            throw new Exception("Received unexpected response type from server.");
        }
    }

    private void startReaderThread() {
        Thread reader = new Thread(() -> {
            try {
                while (true) {
                    Response response = Response.parseDelimitedFrom(input);
                    if (response == null) break; // Server disconnected

                    if (response.getPayloadCase() == Response.PayloadCase.UPDATE) {
                        if (observer != null) {
                            MatchDto dto = response.getUpdate().getUpdatedMatch();

                            Team teamA = new Team(dto.getTeamA().getTeamId(), dto.getTeamA().getName());
                            Team teamB = new Team(dto.getTeamB().getTeamId(), dto.getTeamB().getName());
                            Stadium stadium = new Stadium(dto.getStadium().getStadiumId(), dto.getStadium().getName(), dto.getStadium().getCapacity());

                            Match match = new Match(dto.getMatchId(), teamA, teamB, stadium, dto.getTicketPrice(), dto.getNumberOfSeatsAvailable());

                            observer.matchUpdated(match);
                        }
                    } else {
                        // It's a direct answer to a request (Login, GetMatches, BuyTicket).
                        responses.put(response);
                    }
                }
            } catch (Exception e) {
                System.out.println("Disconnected from server or error reading: " + e.getMessage());
            }
        });
        reader.setDaemon(true); // Ensures the thread dies when the app closes
        reader.start();
    }
}