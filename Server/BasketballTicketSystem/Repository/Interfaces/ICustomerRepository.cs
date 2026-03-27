using System.Collections.Generic;

public interface ICustomerRepository : IRepository<int, ICustomer>
{
    ICustomer FindByName(string name);
}