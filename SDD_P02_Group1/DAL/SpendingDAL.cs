using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SDD_P02_Group1.Models;

namespace SDD_P02_Group1.DAL
{
    public class SpendingDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public SpendingDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "MoolahConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Spending> GetSpendingByWeek(int userid,DateTime date)
        {
            List<Spending> spendingList = new List<Spending>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserWeeklySpending WHERE UserID = @id AND FirstDateOfWeek = @date";
            cmd.Parameters.AddWithValue("@id", userid);
            cmd.Parameters.AddWithValue("@date", date);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    spendingList.Add(
                    new Spending
                    {
                        SpendingID = reader.GetInt32(0),
                        FirstDateOfWeek = reader.GetDateTime(1),
                        MonSpending = !reader.IsDBNull(2) ? reader.GetDecimal(2) : (decimal?)null,
                        TueSpending = !reader.IsDBNull(3) ? reader.GetDecimal(3) : (decimal?)null,
                        WedSpending = !reader.IsDBNull(4) ? reader.GetDecimal(4) : (decimal?)null,
                        ThuSpending = !reader.IsDBNull(5) ? reader.GetDecimal(5) : (decimal?)null,
                        FriSpending = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null,
                        SatSpending = !reader.IsDBNull(7) ? reader.GetDecimal(7) : (decimal?)null,
                        SunSpending = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null,
                        TotalSpending = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null,
                        UserID = userid,
                    }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return spendingList;
        }

        public List<Spending> GetAllSpending(int userid)
        {
            List<Spending> spendingList = new List<Spending>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserWeeklySpending WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", userid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    spendingList.Add(
                    new Spending
                    {
                        SpendingID = reader.GetInt32(0),
                        FirstDateOfWeek = reader.GetDateTime(1),
                        MonSpending = !reader.IsDBNull(2) ? reader.GetDecimal(2) : (decimal?)null,
                        TueSpending = !reader.IsDBNull(3) ? reader.GetDecimal(3) : (decimal?)null,
                        WedSpending = !reader.IsDBNull(4) ? reader.GetDecimal(4) : (decimal?)null,
                        ThuSpending = !reader.IsDBNull(5) ? reader.GetDecimal(5) : (decimal?)null,
                        FriSpending = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null,
                        SatSpending = !reader.IsDBNull(7) ? reader.GetDecimal(7) : (decimal?)null,
                        SunSpending = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null,
                        TotalSpending = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null,
                        UserID = userid,
                    }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return spendingList;
        }
    }
}
