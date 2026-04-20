package View;

import Controller.MainController;
import Model.Match;
import Networking.ServerProxy;
import Service.EmployeeService;
import Service.MatchService;
import Service.TicketService;
import javafx.application.Application;
import javafx.beans.property.SimpleStringProperty;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.*;
import javafx.stage.Stage;

import java.util.List;

public class MainApp extends Application {

    private MainController controller;
    private ServerProxy proxy;

    // UI Components
    private TableView<Match> matchTable;
    private VBox ticketingSection;
    private Label statusLabel;

    @Override
    public void start(Stage primaryStage) {
        // 1. Initialize and connect the Proxy
        proxy = new ServerProxy("127.0.0.1", 5555);
        try {
            proxy.connect();
            System.out.println("Successfully connected to the C# Server.");
        } catch (Exception e) {
            System.err.println("Failed to connect to server! Is the C# server running?");
            e.printStackTrace();
        }

        // 2. Initialize Services with the Proxy (No more InMemory Repositories!)
        EmployeeService employeeService = new EmployeeService(proxy);
        MatchService matchService = new MatchService(proxy);
        TicketService ticketService = new TicketService(proxy);

        // 3. Initialize Controller
        controller = new MainController(employeeService, matchService, ticketService);

        // Build the UI
        BorderPane root = new BorderPane();
        root.setPadding(new Insets(10));

        // TOP
        HBox loginSection = new HBox(10);
        TextField userField = new TextField();
        userField.setPromptText("Username (e.g., ingat29)");
        PasswordField passField = new PasswordField();
        passField.setPromptText("Password (e.g., pass)");
        Button loginBtn = new Button("Login");
        Button logoutBtn = new Button("Logout");
        logoutBtn.setDisable(true);
        Label userLabel = new Label("Not logged in");

        loginSection.getChildren().addAll(new Label("Login:"), userField, passField, loginBtn, logoutBtn, userLabel);
        root.setTop(loginSection);

        // CENTER
        matchTable = new TableView<>();

        TableColumn<Match, String> teamACol = new TableColumn<>("Team A");
        teamACol.setCellValueFactory(data -> new SimpleStringProperty(data.getValue().getTeamA().getName()));

        TableColumn<Match, String> teamBCol = new TableColumn<>("Team B");
        teamBCol.setCellValueFactory(data -> new SimpleStringProperty(data.getValue().getTeamB().getName()));

        TableColumn<Match, String> stadiumCol = new TableColumn<>("Stadium");
        stadiumCol.setCellValueFactory(data -> new SimpleStringProperty(data.getValue().getStadium().getName()));

        TableColumn<Match, String> priceCol = new TableColumn<>("Price");
        priceCol.setCellValueFactory(data -> new SimpleStringProperty("$" + data.getValue().getTicketPrice()));

        TableColumn<Match, String> seatsCol = new TableColumn<>("Available Seats");
        seatsCol.setCellValueFactory(data -> new SimpleStringProperty(String.valueOf(data.getValue().getAvailableSeats())));

        matchTable.getColumns().addAll(teamACol, teamBCol, stadiumCol, priceCol, seatsCol);
        root.setCenter(matchTable);

        // BOTTOM
        ticketingSection = new VBox(10);
        ticketingSection.setPadding(new Insets(10, 0, 0, 0));
        ticketingSection.setDisable(true); // Disabled until logged in

        HBox buyBox = new HBox(10);
        TextField customerIdField = new TextField();
        customerIdField.setPromptText("Customer ID (e.g., 1)");
        Spinner<Integer> seatSpinner = new Spinner<>(1, 10, 1);
        Button buyBtn = new Button("Buy Ticket");

        buyBox.getChildren().addAll(new Label("Customer ID:"), customerIdField, new Label("Seats:"), seatSpinner, buyBtn);

        statusLabel = new Label("Please log in to sell tickets.");
        ticketingSection.getChildren().addAll(buyBox, statusLabel);

        root.setBottom(ticketingSection);

        // Button Actions
        loginBtn.setOnAction(e -> {
            boolean success = controller.login(userField.getText(), passField.getText());
            if (success) {
                userLabel.setText("Logged in as: " + controller.getLoggedInEmployee().getUsername());
                ticketingSection.setDisable(false);
                loginBtn.setDisable(true);
                logoutBtn.setDisable(false);
                statusLabel.setText("Login successful. Select a match to sell tickets.");
                refreshTable();
            } else {
                statusLabel.setText("Invalid credentials or server offline!");
            }
        });

        logoutBtn.setOnAction(e -> {
            controller.logout();
            userLabel.setText("Not logged in");
            ticketingSection.setDisable(true);
            loginBtn.setDisable(false);
            logoutBtn.setDisable(true);
            statusLabel.setText("Logged out successfully.");
            matchTable.getItems().clear(); // Hide matches if not logged in
        });

        buyBtn.setOnAction(e -> {
            Match selectedMatch = matchTable.getSelectionModel().getSelectedItem();
            try {
                int customerId = Integer.parseInt(customerIdField.getText());

                controller.buyTicket(selectedMatch, customerId, seatSpinner.getValue());

                statusLabel.setText("Ticket purchased successfully for Customer ID " + customerId + "!");
                refreshTable();
                customerIdField.clear();

            } catch (NumberFormatException nfe) {
                statusLabel.setText("Error: Customer ID must be a valid number!");
            } catch (Exception ex) {
                statusLabel.setText("Error: " + ex.getMessage());
            }
        });

        Scene scene = new Scene(root, 800, 500);
        primaryStage.setTitle("Basketball Ticket System (HA-3)");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    private void refreshTable() {
        matchTable.getItems().clear();
        List<Match> matches = controller.getAvailableMatches();
        if (matches != null) {
            matchTable.getItems().addAll(matches);
        }
    }

    // This guarantees the Java app hangs up the socket when you click the 'X' button!
    @Override
    public void stop() throws Exception {
        if (proxy != null) {
            proxy.disconnect();
        }
        super.stop();
    }

    public static void main(String[] args) {
        launch(args);
    }
}