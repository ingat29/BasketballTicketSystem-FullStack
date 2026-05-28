import React, { useState, useEffect } from 'react';
// Import the SignalR connection manager engine we installed in Step 1
import * as signalR from '@microsoft/signalr'; 

function App() {
  const [matches, setMatches] = useState([]);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({ matchId: '', teamAId: '', teamBId: '', stadiumId: '', numberOfSeatsAvailable: '', ticketPrice: '' });

  const API_BASE_URL = "http://localhost:59579/api/matches";

  // 1. READ INITIAL RECORDS ON LOAD
  const loadMatches = async () => {
    try {
      const response = await fetch(API_BASE_URL);
      if (!response.ok) throw new Error("Failed to pull match data.");
      const data = await response.json();
      setMatches(data);
    } catch (error) {
      console.error("Error connecting to C# backend:", error);
    }
  };

  // 2. LIFECYCLE MANAGEMENT: INITIAL DATA LOAD & WEBSOCKET SUBSCRIPTION
  useEffect(() => {
    // A. Perform our baseline load
    loadMatches();

    // B. Build and configure our permanent SignalR WebSocket tunnel
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:59579/notificationHub") // Must match our app.MapHub server path exactly
      .withAutomaticReconnect() // Auto-repair the pipe if local Wi-Fi drops out momentarily
      .build();

    // C. Fire up the connection pipeline
    connection.start()
      .then(() => console.log("SignalR Connection Tunnel established successfully."))
      .catch(err => console.error("SignalR Connection Failure: ", err));

    // D. REGISTER THE LIVE EVENT RECEIVER (Acts exactly like your old Java TicketObserver!)
    connection.on("MatchSystemChanged", (message) => {
      console.log("Real-time push event captured from server:", message);
      
      const { action, payload } = message;

      // Leverage React reactive state updating loops depending on server notification rules
      if (action === "ADD") {
        // Append the new match directly to our list container memory
        setMatches(prevMatches => [...prevMatches, payload]);
      } 
      else if (action === "MODIFY") {
        // Swap out only the modified entry by identifying its matchId tracking signature
        setMatches(prevMatches => prevMatches.map(m => m.matchId === payload.matchId ? payload : m));
      } 
      else if (action === "DELETE") {
        // Strip the removed record out of our visual view array filter list
        setMatches(prevMatches => prevMatches.filter(m => m.matchId !== payload.matchId));
      }
    });

    // E. CLEANUP TRIGGER: Shut down the socket channel if the user closes the website tab
    return () => {
      connection.stop();
    };
  }, []);

  // 3. EVENT FORM CHANGES HANDLER
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === "ticketPrice" ? parseFloat(value) : parseInt(value) || value
    });
  };

  // 4. ACTION SUBMIT (REST HTTP REQUESTS)
  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      if (isEditing) {
        const response = await fetch(`${API_BASE_URL}/${formData.matchId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(formData)
        });
        if (!response.ok) throw new Error("Failed to update match profile.");
        setIsEditing(false);
      } else {
        const { matchId, ...newMatchData } = formData;
        const response = await fetch(API_BASE_URL, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(newMatchData)
        });
        if (!response.ok) throw new Error("Failed to introduce match entry.");
      }

      setFormData({ matchId: '', teamAId: '', teamBId: '', stadiumId: '', numberOfSeatsAvailable: '', ticketPrice: '' });

    } catch (error) {
      alert("Action Error: " + error.message);
    }
  };

  // 5. ACTION REMOVE (REST HTTP REQUEST)
  const handleDelete = async (id) => {
    if (!window.confirm(`Delete Match ${id}?`)) return;
    try {
      const response = await fetch(`${API_BASE_URL}/${id}`, { method: 'DELETE' });
      if (!response.ok) throw new Error("Server rejected deletion operations.");
      // Again, no manual reloading. The server's broadcast handles the visual change.
    } catch (error) {
      alert("Deletion error: " + error.message);
    }
  };

  const startEdit = (match) => {
    setFormData(match);
    setIsEditing(true);
  };

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif', maxWidth: '1000px', margin: '0 auto' }}>
      <h1>Basketball Ticket System - Real-Time Dashboard</h1>
      <p style={{ color: '#2196F3', fontWeight: 'bold' }}>⚡ Live Update Synchronization Tunnel Active</p>
      <hr />

      {/* ADMIN CONTROL SECTION FORM */}
      <div style={{ backgroundColor: '#f9f9f9', padding: '20px', borderRadius: '5px', marginBottom: '25px', border: '1px solid #ddd' }}>
        <h3>{isEditing ? `Modify Match Profile (ID: ${formData.matchId})` : "Create New Basketball Match"}</h3>
        <form onSubmit={handleSubmit} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px' }}>
          <div>
            <label>Team A ID:</label>
            <input type="number" name="teamAId" value={formData.teamAId} onChange={handleInputChange} style={{ width: '100%', padding: '8px' }} required />
          </div>
          <div>
            <label>Team B ID:</label>
            <input type="number" name="teamBId" value={formData.teamBId} onChange={handleInputChange} style={{ width: '100%', padding: '8px' }} required />
          </div>
          <div>
            <label>Stadium ID:</label>
            <input type="number" name="stadiumId" value={formData.stadiumId} onChange={handleInputChange} style={{ width: '100%', padding: '8px' }} required />
          </div>
          <div>
            <label>Available Seats:</label>
            <input type="number" name="numberOfSeatsAvailable" value={formData.numberOfSeatsAvailable} onChange={handleInputChange} style={{ width: '100%', padding: '8px' }} required />
          </div>
          <div style={{ gridColumn: 'span 2' }}>
            <label>Ticket Price ($):</label>
            <input type="number" step="0.01" name="ticketPrice" value={formData.ticketPrice} onChange={handleInputChange} style={{ width: '100%', padding: '8px' }} required />
          </div>
          <div style={{ gridColumn: 'span 2', marginTop: '10px' }}>
            <button type="submit" style={{ padding: '10px 20px', backgroundColor: isEditing ? '#2196F3' : '#4CAF50', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginRight: '10px' }}>
              {isEditing ? "Save Changes" : "Register Match"}
            </button>
            {isEditing && (
              <button type="button" onClick={() => { setIsEditing(false); setFormData({ matchId: '', teamAId: '', teamBId: '', stadiumId: '', numberOfSeatsAvailable: '', ticketPrice: '' }); }} style={{ padding: '10px 20px', backgroundColor: '#f44336', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
                Cancel
              </button>
            )}
          </div>
        </form>
      </div>

      {/* GRID DATA LIST TABLE */}
      <h3>Available Matches</h3>
      {matches.length === 0 ? (
        <p style={{ color: '#666' }}>No matches found in the database.</p>
      ) : (
        <table border="1" cellPadding="10" style={{ borderCollapse: 'collapse', width: '100%' }}>
          <thead>
            <tr style={{ backgroundColor: '#f2f2f2', textAlign: 'left' }}>
              <th>Match ID</th><th>Team A ID</th><th>Team B ID</th><th>Stadium ID</th><th>Available Seats</th><th>Ticket Price</th><th style={{ textAlign: 'center' }}>Management Actions</th>
            </tr>
          </thead>
          <tbody>
            {matches.map((match) => (
              <tr key={match.matchId}>
                <td><strong>{match.matchId}</strong></td>
                <td>{match.teamAId}</td>
                <td>{match.teamBId}</td>
                <td>{match.stadiumId}</td>
                <td>{match.numberOfSeatsAvailable}</td>
                <td>${match.ticketPrice.toFixed(2)}</td>
                <td style={{ textAlign: 'center' }}>
                  <button onClick={() => startEdit(match)} style={{ marginRight: '8px', padding: '5px 12px', backgroundColor: '#ff9800', color: 'white', border: 'none', borderRadius: '3px', cursor: 'pointer' }}>Modify</button>
                  <button onClick={() => handleDelete(match.matchId)} style={{ padding: '5px 12px', backgroundColor: '#f44336', color: 'white', border: 'none', borderRadius: '3px', cursor: 'pointer' }}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;