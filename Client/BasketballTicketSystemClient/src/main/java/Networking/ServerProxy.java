package Networking;

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

    public ServerProxy(String host, int port) {
        this.host = host;
        this.port = port;
    }

    // =================================================================
    // 1. CONNECTION MANAGEMENT
    // =================================================================

    public void connect() throws Exception {
        if (connection == null || connection.isClosed()) {
            connection = new Socket(host, port);
            output = connection.getOutputStream();
            input = connection.getInputStream();
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

    // =================================================================
    // 2. NETWORK ACTIONS (The Translators)
    // =================================================================

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
        Response response = Response.parseDelimitedFrom(input);

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
        // Pack
        Request request = Request.newBuilder()
                .setGetMatches(
                        GetMatchesRequest.newBuilder()
                                .setMinSeats(minSeats)
                                .build()
                )
                .build();

        // Send & Wait
        request.writeDelimitedTo(output);
        Response response = Response.parseDelimitedFrom(input);

        // Unpack
        if (response.getPayloadCase() == Response.PayloadCase.GET_MATCHES) {
            GetMatchesResponse matchResp = response.getGetMatches();
            List<Match> javaMatches = new ArrayList<>();

            // THE TRANSLATION LAYER: DTO -> Real Java Models
            for (MatchDto dto : matchResp.getMatchesList()) {

                // 1. Unpack the nested Team A
                Team teamA = new Team(dto.getTeamA().getTeamId(), dto.getTeamA().getName());

                // 2. Unpack the nested Team B
                Team teamB = new Team(dto.getTeamB().getTeamId(), dto.getTeamB().getName());

                // 3. Unpack the nested Stadium
                Stadium stadium = new Stadium(
                        dto.getStadium().getStadiumId(),
                        dto.getStadium().getName(),
                        dto.getStadium().getCapacity()
                );

                // 4. Construct the final Java Match object!
                // NOTE: Ensure these parameters match your exact Match.java constructor
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
        // Pack
        Request request = Request.newBuilder()
                .setBuyTicket(
                        BuyTicketRequest.newBuilder()
                                .setMatchId(matchId)
                                .setCustomerId(customerId)
                                .setNumberOfSeats(numberOfSeats)
                                .build()
                )
                .build();

        // Send & Wait
        request.writeDelimitedTo(output);
        Response response = Response.parseDelimitedFrom(input);

        // Unpack
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
}