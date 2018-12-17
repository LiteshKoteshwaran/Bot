using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LRKElectronics
{
    [Serializable]
    public class DAL
    {
        DataTable dataTable;
        //ConnetionMannger connetionMannger;
        public List<string> GetCategory(string Query)
        {
            ConnetionMannger connetionMannger = new ConnetionMannger();
            List<string> Items = new List<string>();
            dataTable = connetionMannger.DataTableConnection(Query);
            foreach (DataRow dr in dataTable.Rows)
            {
                string Item = dr[0].ToString();
                Items.Add(Item);
            }
            return Items;
        }

        public string GetPrice(string Query)
        {
            string Item = "";
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnetionMannger.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(Query, connection);
                try
                {
                    connection.Open();
                    Item = cmd.ExecuteScalar().ToString();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return Item;
        }
    }
}