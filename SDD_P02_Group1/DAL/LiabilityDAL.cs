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
    public class LiabilityDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public LiabilityDAL()
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

        public int AddLiability(Liability liability, int userID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO [] (LiabilityName, Description, DueDate, AmountDue, UserID) OUTPUT INSERTED.LiabilityID VALUES (@name, @description, @date, @amount, @userid)";
            cmd.Parameters.AddWithValue("@name", liability.LiabilityName);
            cmd.Parameters.AddWithValue("@description", liability.Description);
            cmd.Parameters.AddWithValue("@date", liability.DueDate);
            cmd.Parameters.AddWithValue("@amount", liability.AmountDue);
            cmd.Parameters.AddWithValue("@userid", userID);


            //A connection to database must be opened before any operations made.
            conn.Open();
            liability.LiabilityID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return liability.LiabilityID;
        }

        public Liability GetLiabilityDetails(int liabilityId)
        {
            Liability liability = new Liability();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM [] WHERE LiabilityID = @id";
            cmd.Parameters.AddWithValue("@id", liabilityId);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    liability.LiabilityID = liabilityId;
                    liability.LiabilityName = reader.GetString(1);
                    liability.Description = reader.GetString(2);
                    liability.DueDate = reader.GetDateTime(3);
                    liability.AmountDue = reader.GetDecimal(4);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return liability;
        }
    }
}
