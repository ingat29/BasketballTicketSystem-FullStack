using System.Collections.Generic;

public interface IEmployeeRepository : IRepository<string, IEmployee>
{
    IEmployee FindByUsernameAndPassword(string username, string password);
}