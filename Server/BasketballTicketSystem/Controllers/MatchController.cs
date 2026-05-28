using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BasketballTicketSystem.Hubs;
using System;
using System.Collections.Generic;

namespace BasketballTicketSystem.Controllers {
    // This tells the router that URLs for this class start with "api/matches"
    [Route("api/[controller]")]
    // This tells the framework this class handles web API requests
    [ApiController]
    public class MatchesController : ControllerBase {
        private readonly IMatchRepository _matchRepo;
        private readonly IHubContext<NotificationHub> _hubContext;

        public MatchesController(IMatchRepository matchRepo, IHubContext<NotificationHub> hubContext) {
            _matchRepo = matchRepo;
            _hubContext = hubContext;
        }

        private async Task BroadcastChange(string changeType, object data) {
            await _hubContext.Clients.All.SendAsync("MatchSystemChanged", new { Action = changeType, Payload = data });
        }

        // GET: api/matches
        [HttpGet]
        public ActionResult<List<IMatch>> GetAllMatches() {
            var matches = _matchRepo.FindAll();
            return Ok(matches); // Returns HTTP 200
        }

        // GET: api/matches/id
        [HttpGet("{id}")]
        public ActionResult<IMatch> GetMatchById(int id) {
            var match = _matchRepo.FindById(id);

            if (match == null) {
                return NotFound($"Match with ID {id} not found."); // Returns HTTP 404
            }

            return Ok(match); // Returns HTTP 200
        }

        // POST: api/matches
        [HttpPost]
        public async Task<ActionResult<IMatch>> CreateMatch([FromBody] Match newMatch) {
            var createdMatch = _matchRepo.Add(newMatch);

            // BROADCAST THE ADD EVENT TO ALL BROWSERS
            await BroadcastChange("ADD", createdMatch);

            return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.matchId }, createdMatch);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IMatch>> UpdateMatch(int id, [FromBody] Match updatedMatch) {
            if (id != updatedMatch.matchId) return BadRequest();
            var existingMatch = _matchRepo.FindById(id);
            if (existingMatch == null) return NotFound();

            var result = _matchRepo.Update(updatedMatch);

            // BROADCAST THE MODIFICATION EVENT TO ALL BROWSERS
            await BroadcastChange("MODIFY", result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id) {
            var existingMatch = _matchRepo.FindById(id);
            if (existingMatch == null) return NotFound();

            try {
                _matchRepo.Delete(id);

                // BROADCAST THE DELETION EVENT TO ALL BROWSERS
                await BroadcastChange("DELETE", new { matchId = id });

                return NoContent();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}