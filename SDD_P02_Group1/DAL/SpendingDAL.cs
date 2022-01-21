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

        public Spending GetSpendingByDate(int userid,DateTime date)
        {
            Spending spending = new Spending();
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
                    spending.SpendingID = reader.GetInt32(0);
                    spending.FirstDateOfWeek = reader.GetDateTime(1);
                    spending.MonSpending = !reader.IsDBNull(2) ? reader.GetDecimal(2) : (decimal?)null;
                    spending.TueSpending = !reader.IsDBNull(3) ? reader.GetDecimal(3) : (decimal?)null;
                    spending.WedSpending = !reader.IsDBNull(4) ? reader.GetDecimal(4) : (decimal?)null;
                    spending.ThuSpending = !reader.IsDBNull(5) ? reader.GetDecimal(5) : (decimal?)null;
                    spending.FriSpending = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null;
                    spending.SatSpending = !reader.IsDBNull(7) ? reader.GetDecimal(7) : (decimal?)null;
                    spending.SunSpending = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null;
                    spending.TotalSpending = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null;
                    spending.UserID = userid;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return spending;
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

        public int UpdateWeeklySpending(int userid, SpendingRecord record)
        {

            int day = (int)record.DateOfTransaction.DayOfWeek;
            string[] dayname = { "SunSpending", "MonSpending", "TueSpending", "WedSpending", "ThuSpending", "FriSpending", "SatSpending" };

            DateTime today = DateTime.Today;
            while (today.DayOfWeek.ToString() != "Monday")
            {
                today = today.AddDays(-1);
            }

            Spending spending = GetSpendingByDate(userid, today);
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            if (dayname[day] == "MonSpending")
            {
                if (spending.MonSpending is null)
                {
                    spending.MonSpending = record.AmountSpent;
                }
                else
                {
                    spending.MonSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET MonSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.MonSpending);
            }
            else if (dayname[day] == "TueSpending")
            {
                if (spending.TueSpending is null)
                {
                    spending.TueSpending = record.AmountSpent;
                }
                else
                {
                    spending.TueSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET TueSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.TueSpending);
            }
            else if (dayname[day] == "WedSpending")
            {
                if (spending.WedSpending is null)
                {
                    spending.WedSpending = record.AmountSpent;
                }
                else
                {
                    spending.WedSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET WedSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.WedSpending);
            }
            else if (dayname[day] == "ThuSpending")
            {
                if (spending.ThuSpending is null)
                {
                    spending.ThuSpending = record.AmountSpent;
                }
                else
                {
                    spending.ThuSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET ThuSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.ThuSpending);
            }
            else if (dayname[day] == "FriSpending")
            {
                if (spending.FriSpending is null)
                {
                    spending.FriSpending = record.AmountSpent;
                }
                else
                {
                    spending.FriSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET FriSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.FriSpending);
            }
            else if (dayname[day] == "SatSpending")
            {
                if (spending.SatSpending is null)
                {
                    spending.SatSpending = record.AmountSpent;
                }
                else
                {
                    spending.SatSpending += record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET SatSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.SatSpending);
            }
            else if (dayname[day] == "SunSpending")
            {
                if (spending.SunSpending.HasValue)
                {
                    spending.SunSpending += record.AmountSpent;
                }
                else
                {
                    spending.SunSpending = record.AmountSpent;
                }
                cmd.CommandText = @"UPDATE UserWeeklySpending SET SunSpending = @value, TotalSpending = @totalvalue WHERE FirstDateOfWeek = @date AND UserID = @id";
                cmd.Parameters.AddWithValue("@value", spending.SunSpending);
            }
            
            if (spending.TotalSpending is null)
            {
                spending.TotalSpending = record.AmountSpent;
            }
            else
            {
                spending.TotalSpending += record.AmountSpent;
            }
            cmd.Parameters.AddWithValue("@totalvalue", spending.TotalSpending);
            cmd.Parameters.AddWithValue("@date", spending.FirstDateOfWeek);
            cmd.Parameters.AddWithValue("@id", spending.UserID);


            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public bool IsSpendingExist(int userId,DateTime date)
        {
            bool spendingFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a spending record with date
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT FirstDateOfWeek FROM UserWeeklySpending WHERE FirstDateOfWeek=@date";
            cmd.Parameters.AddWithValue("@date", date);

            //Open a database connection and execute the SQL statement
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                spendingFound = true;
            }

            else
            { //No record
                spendingFound = false; // The email address given does not exist
            }

            reader.Close();
            conn.Close();

            return spendingFound;
        }

        public int AddDefaultSpending(int userID, DateTime date)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated SpendingID after insertion
            cmd.CommandText = @"INSERT INTO UserWeeklySpending (FirstDateOfWeek, MonSpending, TueSpending, WedSpending, ThuSpending, FriSpending, SatSpending, SunSpending, TotalSpending, UserID) OUTPUT INSERTED.SpendingID VALUES (@FirstDateOfWeek, @MonSpending, @TueSpending, @WedSpending, @ThuSpending, @FriSpending, @SatSpending, @SunSpending, @TotalSpending, @UserID)";
            cmd.Parameters.AddWithValue("@FirstDateOfWeek", date);
            cmd.Parameters.AddWithValue("@MonSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@TueSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@WedSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@ThuSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@FriSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@SatSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@SunSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalSpending", DBNull.Value);
            cmd.Parameters.AddWithValue("@UserID", userID);

            //A connection to database must be opened before any operations made.
            conn.Open();
            int SpendingID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return SpendingID;
        }

        public int EditSpending(WeeklySpending spending)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated SpendingID after insertion
            cmd.CommandText = @"UPDATE UserWeeklySpending SET MonSpending=@mon, TueSpending=@tue, WedSpending=@wed, ThuSpending=@thu, FriSpending=@fri, SatSpending=@sat, SunSpending=@sun, TotalSpending=@total WHERE FirstDateOfWeek = @date AND UserID = @id";
            cmd.Parameters.AddWithValue("@date", spending.FirstDateOfWeek);
            cmd.Parameters.AddWithValue("@mon", spending.MonSpending);
            cmd.Parameters.AddWithValue("@tue", spending.TueSpending);
            cmd.Parameters.AddWithValue("@wed", spending.WedSpending);
            cmd.Parameters.AddWithValue("@thu", spending.ThuSpending);
            cmd.Parameters.AddWithValue("@fri", spending.FriSpending);
            cmd.Parameters.AddWithValue("@sat", spending.SatSpending);
            cmd.Parameters.AddWithValue("@sun", spending.SunSpending);
            cmd.Parameters.AddWithValue("@total", spending.TotalSpending);
            cmd.Parameters.AddWithValue("@id", spending.UserID);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public List<SpendingRecord> GetAllSpendingRecord(int userid)
        {
            List<SpendingRecord> recordList = new List<SpendingRecord>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserSpendingRecord WHERE UserID = @id ORDER BY DateOfTransaction ASC";
            cmd.Parameters.AddWithValue("@id", userid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    recordList.Add(
                    new SpendingRecord
                    {
                        RecordID = reader.GetInt32(0),
                        DateOfTransaction = reader.GetDateTime(1),
                        CategoryOfSpending = reader.GetString(2),
                        AmountSpent = reader.GetDecimal(3),
                        UserID = userid,
                    }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return recordList;
        }

        public List<SpendingRecord> GetSpendingRecordByDate(int userid, DateTime date)
        {
            List<SpendingRecord> recordList = new List<SpendingRecord>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserSpendingRecord WHERE UserID = @id AND DateOfTransaction = @date";
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
                    recordList.Add(
                    new SpendingRecord
                    {
                        RecordID = reader.GetInt32(0),
                        DateOfTransaction = reader.GetDateTime(1),
                        CategoryOfSpending = reader.GetString(2),
                        AmountSpent = reader.GetDecimal(3),
                        UserID = userid,
                    }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return recordList;
        }

        public SpendingRecord GetSpendingRecordByID(int recordid)
        {
            SpendingRecord record = new SpendingRecord();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserSpendingRecord WHERE RecordID = @recordid";
            cmd.Parameters.AddWithValue("@recordid", recordid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    record.RecordID = recordid;
                    record.DateOfTransaction = reader.GetDateTime(1);
                    record.CategoryOfSpending = reader.GetString(2);
                    record.AmountSpent = reader.GetDecimal(3);
                    record.UserID = reader.GetInt32(4);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return record;
        }

        public int AddSpending(SpendingRecord record, int userid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated RecordID after insertion
            cmd.CommandText = @"INSERT INTO UserSpendingRecord (DateOfTransaction, CategoryOfSpending, AmountSpent, UserID) OUTPUT INSERTED.RecordID VALUES (@date,@cat,@amt,@userid)";

            cmd.Parameters.AddWithValue("@date", record.DateOfTransaction);
            cmd.Parameters.AddWithValue("@cat", record.CategoryOfSpending);
            cmd.Parameters.AddWithValue("@amt", record.AmountSpent);
            cmd.Parameters.AddWithValue("@userid", userid);



            //A connection to database must be opened before any operations made.
            conn.Open();
            record.RecordID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return record.RecordID;
        }
    }
}
