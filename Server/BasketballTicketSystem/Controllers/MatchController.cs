using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BasketballTicketSystem.Controllers {
    // This tells the router that URLs for this class start with "api/matches"
    [Route("api/[controller]")]
    // This tells the framework this class handles web API requests
    [ApiController]
    public class MatchesController : ControllerBase {
        private readonly IMatchRepository _matchRepo;

        public MatchesController(IMatchRepository matchRepo) {
            _matchRepo = matchRepo;
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
        public ActionResult<IMatch> CreateMatch([FromBody] Match newMatch) {
            // The client should send JSON without a "matchId" 
            var createdMatch = _matchRepo.Add(newMatch);

            // Returns an HTTP 201 (Created)
            return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.matchId }, createdMatch);
        }

        // PUT: api/matches/id
        [HttpPut("{id}")]
        public ActionResult<IMatch> UpdateMatch(int id, [FromBody] Match updatedMatch) {
            if (id != updatedMatch.matchId) {
                return BadRequest("The ID in the URL does not match the ID in the body."); // HTTP 400
            }

            var existingMatch = _matchRepo.FindById(id);
            if (existingMatch == null) {
                return NotFound(); // HTTP 404
            }

            var result = _matchRepo.Update(updatedMatch);
            return Ok(result); // HTTP 200
        }

        // DELETE: api/matches/id
        [HttpDelete("{id}")]
        public ActionResult DeleteMatch(int id) {
            var existingMatch = _matchRepo.FindById(id);
            if (existingMatch == null) {
                return NotFound(); // HTTP 404
            }

            try {
                _matchRepo.Delete(id);
                return NoContent(); // HTTP 204 (Delete)
            }
            catch (Exception ex) {
                return BadRequest(ex.Message); // HTTP 400
            }
        }
    }
}