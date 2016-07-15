using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WCFMemoryLeak1.Service.Interface;

namespace WCFMemoryLeak1.Service
{
    public class DataAccessService : IDataAccessService
    {
        private SqlConnection _sqlConnection = null;
        private SqlCommand _sqlCommand = null;
        private SqlDataAdapter _sqlDataAdapter = null;

        public DataAccessService()
        {
        }

        #region GetAddressesByCustomerId
        private const string sqlGetAddressesByCustomerId = @"SELECT A.[AddressID],CA.[AddressType],A.[AddressLine1],A.[AddressLine2],A.[City],A.[StateProvince],A.[CountryRegion],A.[PostalCode] FROM [SalesLT].[Address] A INNER JOIN [SalesLT].[CustomerAddress] CA ON CA.AddressId = A.AddressID WHERE CA.CustomerId = @customerId";
        public DataTable GetAddressesByCustomerId(int customerId)
        {
            var sqlParameters = new SqlParameter[1]
            {
                new SqlParameter("customerId", customerId)
            };

            return ExecuteQueryWithParams(sqlGetAddressesByCustomerId, sqlParameters);
        }
        #endregion

        #region GetCustomerById
        private const string sqlGetCustomerById = @"SELECT [CustomerID],[Title],[FirstName],[MiddleName],[LastName],[Suffix],[EmailAddress],[Phone] FROM [SalesLT].[Customer] WHERE [CustomerID] = @customerId";
        public DataTable GetCustomerById(int customerId)
        {
            var sqlParameters = new SqlParameter[1]
            {
                new SqlParameter("customerId", customerId)
            };

            return ExecuteQueryWithParams(sqlGetCustomerById, sqlParameters);
        }
        #endregion

        #region GetCustomersByCountry
        private const string sqlGetCustomersByCountry = @"SELECT C.[CustomerID],C.[Title],C.[FirstName],C.[MiddleName],C.[LastName],C.[Suffix],C.[EmailAddress],C.[Phone] FROM [SalesLT].[Customer] C INNER JOIN [SalesLT].[CustomerAddress] CA ON CA.CustomerID = C.CustomerID INNER JOIN [SalesLT].[Address] A ON A.AddressID = CA.AddressID WHERE A.CountryRegion = @country";
        public DataTable GetCustomersByCountry(string country)
        {
            var sqlParameters = new SqlParameter[1]
            {
                new SqlParameter("country", country)
            };

            return ExecuteQueryWithParams(sqlGetCustomersByCountry, sqlParameters);
        }
        #endregion

        private DataTable ExecuteQueryWithParams(string sqlQuery, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["adventureWorksDb"].ConnectionString;
                _sqlConnection = new SqlConnection(connectionString);
                _sqlCommand = new SqlCommand(sqlQuery, _sqlConnection);
                _sqlCommand.Parameters.AddRange(parameters);
                _sqlConnection.Open();

                // Create and fill data adapter from our sql command
                _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
                _sqlDataAdapter.Fill(dataTable);
            }
            finally
            {
                #region Cleanup
                // ==================================
                // Uncomment the code below for fix
                // ==================================
                //if (_sqlConnection != null)
                //{
                //    _sqlConnection.Close();
                //    _sqlConnection.Dispose();
                //    _sqlConnection = null;
                //}

                //if (_sqlCommand != null)
                //{
                //    _sqlCommand.Dispose();
                //    _sqlCommand = null;
                //}

                //if (_sqlDataAdapter != null)
                //{
                //    _sqlDataAdapter.Dispose();
                //    _sqlDataAdapter = null;
                //}
                #endregion
            }

            return dataTable;
        }
    }
}