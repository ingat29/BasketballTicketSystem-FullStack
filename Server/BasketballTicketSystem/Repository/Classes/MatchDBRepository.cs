using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NLog;

public class MatchDBRepository : IMatchRepository {
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public MatchDBRepository() { logger.Info("Initializing EF MatchDBRepository"); }

    public List<IMatch> FindAllAvailableMatchesOrderedDescending() {
        using (var context = new BasketballContext()) {
            return context.Matches
                .Where(m => m.numberOfSeatsAvailable > 0)
                .OrderByDescending(m => m.numberOfSeatsAvailable)
                .Cast<IMatch>().ToList();
        }
    }

    public List<IMatch> FindAvailableMatchesOrderedDescending(int minSeats) {
        using (var context = new BasketballContext()) {
            return context.Matches
                .Where(m => m.numberOfSeatsAvailable >= minSeats)
                .OrderByDescending(m => m.numberOfSeatsAvailable)
                .Cast<IMatch>().ToList();
        }
    }

    public IMatch Add(IMatch match) {
        using (var context = new BasketballContext()) {
            context.Matches.Add((Match)match);
            context.SaveChanges();
        }
        return match;
    }

    public IMatch FindById(int id) {
        using (var context = new BasketballContext()) {
            return context.Matches.Find(id);
        }
    }

    public List<IMatch> FindAll() {
        using (var context = new BasketballContext()) {
            return context.Matches.Cast<IMatch>().ToList();
        }
    }

    public IMatch Update(IMatch match) {
        using (var context = new BasketballContext()) {
            context.Matches.Update((Match)match);
            context.SaveChanges();
        }
        return match;
    }

    public IMatch Delete(int id) {
        using (var context = new BasketballContext()) {
            var match = context.Matches.Find(id);
            if (match == null) return null;

            try {
                context.Matches.Remove(match);
                context.SaveChanges();
                return match;
            }
            catch (DbUpdateException) {
                // If EF Core throws an exception saving the deletion, it's likely the FK constraint
                logger.Error($"Cannot delete Match {id} because tickets exist.");
                throw new Exception("Cannot delete this match. Tickets exist.");
            }
        }
    }
}