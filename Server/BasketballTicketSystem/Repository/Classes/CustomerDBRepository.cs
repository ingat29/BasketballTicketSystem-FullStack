using System.Collections.Generic;
using System.Linq;

public class CustomerDBRepository : ICustomerRepository {
    public ICustomer FindByName(string name) {
        using (var context = new BasketballContext()) {
            return context.Customers.FirstOrDefault(c => c.fullName == name);
        }
    }
    public ICustomer Add(ICustomer customer) {
        using (var context = new BasketballContext()) { context.Customers.Add((Customer)customer); context.SaveChanges(); return customer; }
    }
    public ICustomer FindById(int id) {
        using (var context = new BasketballContext()) { return context.Customers.Find(id); }
    }
    public List<ICustomer> FindAll() {
        using (var context = new BasketballContext()) { return context.Customers.Cast<ICustomer>().ToList(); }
    }
    public ICustomer Update(ICustomer customer) {
        using (var context = new BasketballContext()) { context.Customers.Update((Customer)customer); context.SaveChanges(); return customer; }
    }
    public ICustomer Delete(int id) {
        using (var context = new BasketballContext()) {
            var cust = context.Customers.Find(id);
            if (cust != null) { context.Customers.Remove(cust); context.SaveChanges(); }
            return cust;
        }
    }
}