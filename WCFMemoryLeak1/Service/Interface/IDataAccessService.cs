using System.Data;

namespace WCFMemoryLeak1.Service.Interface
{
    public interface IDataAccessService
    {
        DataTable GetCustomerById(int customerId);

        DataTable GetCustomersByCountry(string country);

        DataTable GetAddressesByCustomerId(int customerId);
    }
}
