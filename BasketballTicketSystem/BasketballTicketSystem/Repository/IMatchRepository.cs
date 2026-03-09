using System.Collections.Generic;

public interface IMatchRepository : IRepository<int, IMatch>
{
    List<IMatch> FindAllAvailableMatchesOrderedDescending();
    List<Imatch> FindAvailableMatchesOrderedDescending(int minSeats);
}