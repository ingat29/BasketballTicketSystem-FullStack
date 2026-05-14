using System.Collections.Generic;
using System.Linq;

public class StadiumDBRepository : IStadiumRepository {
    public IStadium Add(IStadium stadium) {
        using (var context = new BasketballContext()) { context.Stadiums.Add((Stadium)stadium); context.SaveChanges(); return stadium; }
    }
    public IStadium FindById(int id) {
        using (var context = new BasketballContext()) { return context.Stadiums.Find(id); }
    }
    public List<IStadium> FindAll() {
        using (var context = new BasketballContext()) { return context.Stadiums.Cast<IStadium>().ToList(); }
    }
    public IStadium Update(IStadium stadium) {
        using (var context = new BasketballContext()) { context.Stadiums.Update((Stadium)stadium); context.SaveChanges(); return stadium; }
    }
    public IStadium Delete(int id) {
        using (var context = new BasketballContext()) {
            var st = context.Stadiums.Find(id);
            if (st != null) { context.Stadiums.Remove(st); context.SaveChanges(); }
            return st;
        }
    }
}