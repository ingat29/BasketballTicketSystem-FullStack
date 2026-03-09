using System.Collections.Generic;

public interface ICustomerRepository : IRepository<string, ICustomer>
{
    ICustomer FindByName(string name);
}