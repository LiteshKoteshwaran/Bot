using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class ConnetionMannger
    {
        DataTable dataTable;
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public DataTable DataTableConnection(string Query)
        {
            dataTable = new DataTable();
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

        //public void InsertIntoCart(string ProductName, long Price,string Des)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("InsertIntoCarts", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("ProductName", ProductName);
        //            cmd.Parameters.AddWithValue("TotalPrice", Price);
        //            connection.Open();
        //            cmd.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
        internal bool InsertIntoCart(string str)
        {
            bool isSuccess = true;
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = str;
                    int RowsAffected = cmd.ExecuteNonQuery();
                    if (RowsAffected < 1)
                    {
                        throw new Exception("Could not process the data");
                    }

                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
                finally
                {
                    conn.Close();
                }
                return isSuccess;
            }
        }

    }
}