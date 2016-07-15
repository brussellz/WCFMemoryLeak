using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Data;
using WCFMemoryLeak1.Entities;
using WCFMemoryLeak1.Service.Interface;

namespace WCFMemoryLeak1
{
    public class DataService : IDataService
    {
        public Customer GetCustomerById(int id)
        {
            var customer = new Customer();
            var dataAccessService = ServiceLocator.Current.GetInstance<IDataAccessService>(); //new DataAccessService();
            var dataTable = dataAccessService.GetCustomerById(id);

            // Convert from DataTable to our POCO object
            DataRow row = dataTable.Rows[0];
            customer.Id = int.Parse(row["CustomerID"].ToString());
            customer.FirstName = row["FirstName"].ToString();
            customer.MiddleName = row["MiddleName"].ToString();
            customer.LastName = row["LastName"].ToString();
            customer.Title = row["Title"].ToString();
            customer.Suffix = row["Suffix"].ToString();
            customer.Phone = row["Phone"].ToString();
            customer.Email = row["EmailAddress"].ToString();

            var addressDataTable = dataAccessService.GetAddressesByCustomerId(id);
            foreach (DataRow addressRow in addressDataTable.Rows)
            {
                var address = new Address();
                address.Id = int.Parse(addressRow["AddressID"].ToString());
                address.Type = addressRow["AddressType"].ToString();
                address.Line1 = addressRow["AddressLine1"].ToString();
                address.Line2 = addressRow["AddressLine2"].ToString();
                address.City = addressRow["City"].ToString();
                address.State = addressRow["StateProvince"].ToString();
                address.Zip = addressRow["PostalCode"].ToString();

                customer.Addresses.Add(address);
            }

            return customer;
        }

        public List<Customer> GetCustomersByCountry(string country)
        {
            var customers = new List<Customer>();
            var dataAccessService = ServiceLocator.Current.GetInstance<IDataAccessService>(); //new DataAccessService();
            var dataTable = dataAccessService.GetCustomersByCountry(country);

            // Convert from DataTable to our POCO object
            foreach (DataRow row in dataTable.Rows)
            {
                var customer = new Customer();
                customer.Id = int.Parse(row["CustomerID"].ToString());
                customer.FirstName = row["FirstName"].ToString();
                customer.MiddleName = row["MiddleName"].ToString();
                customer.LastName = row["LastName"].ToString();
                customer.Title = row["Title"].ToString();
                customer.Suffix = row["Suffix"].ToString();
                customer.Phone = row["Phone"].ToString();
                customer.Email = row["EmailAddress"].ToString();

                var addressDataTable = dataAccessService.GetAddressesByCustomerId(customer.Id);
                foreach (DataRow addressRow in addressDataTable.Rows)
                {
                    var address = new Address();
                    address.Id = int.Parse(addressRow["AddressID"].ToString());
                    address.Type = addressRow["AddressType"].ToString();
                    address.Line1 = addressRow["AddressLine1"].ToString();
                    address.Line2 = addressRow["AddressLine2"].ToString();
                    address.City = addressRow["City"].ToString();
                    address.State = addressRow["StateProvince"].ToString();
                    address.Zip = addressRow["PostalCode"].ToString();

                    customer.Addresses.Add(address);
                }

                // Add to our result collection
                customers.Add(customer);
            }

            return customers;
        }
    }
}
