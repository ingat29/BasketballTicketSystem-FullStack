using System.Collections.Generic;
using System.Linq;

public class TeamDBRepository : ITeamRepository {
    public ITeam Add(ITeam team) {
        using (var context = new BasketballContext()) { context.Teams.Add((Team)team); context.SaveChanges(); return team; }
    }
    public ITeam FindById(int id) {
        using (var context = new BasketballContext()) { return context.Teams.Find(id); }
    }
    public List<ITeam> FindAll() {
        using (var context = new BasketballContext()) { return context.Teams.Cast<ITeam>().ToList(); }
    }
    public ITeam Update(ITeam team) {
        using (var context = new BasketballContext()) { context.Teams.Update((Team)team); context.SaveChanges(); return team; }
    }
    public ITeam Delete(int id) {
        using (var context = new BasketballContext()) {
            var tm = context.Teams.Find(id);
            if (tm != null) { context.Teams.Remove(tm); context.SaveChanges(); }
            return tm;
        }
    }
}