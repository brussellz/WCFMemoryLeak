using System.Collections.Generic;
using System.ServiceModel;
using WCFMemoryLeak1.Entities;

namespace WCFMemoryLeak1
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        Customer GetCustomerById(int id);

        [OperationContract]
        List<Customer> GetCustomersByCountry(string country);
    }
}
