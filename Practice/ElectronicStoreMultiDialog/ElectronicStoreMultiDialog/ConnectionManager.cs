using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ElectronicStoreMultiDialog
{
    [Serializable]
    public class ConnectionManager
    {
        DataTable dataTable = new DataTable();
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        public DataTable DataTableConnection(string Query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(Query, connection);
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dataTable;
        }
    }
}