using System;
using System.Data;
using System.Data.SqlClient; 

namespace WinFormsApp2
{
    internal class DbHelper
    {
        // Define the path to your database file
        private static string connectionString = "server=(localdb)\\MSSQLLocalDB;database=MyAppDb3;Trusted_Connection=True;TrustServerCertificate=True";

        public static DataTable GetDataTable(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
