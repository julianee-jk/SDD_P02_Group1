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
            cmd.CommandText = @"INSERT INTO UserLiability (LiabilityName, LiabilityType, LiabilityDesc, Cost, DueDate, RecurringType, RecurringDuration, UserID) OUTPUT INSERTED.LiabilityID VALUES 
(@name, @type, @description, @cost, @date, @rtype, @duration, @userid)";

            cmd.Parameters.AddWithValue("@name", liability.LiabilityName);
            cmd.Parameters.AddWithValue("@type", liability.LiabilityType);
            cmd.Parameters.AddWithValue("@cost", liability.Cost);
            cmd.Parameters.AddWithValue("@rtype", liability.RecurringType);
            cmd.Parameters.AddWithValue("@userid", userID);

            if (liability.LiabilityDesc != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@description", liability.LiabilityDesc);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@description", DBNull.Value);

            if (liability.DueDate != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@date", liability.DueDate);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@date", DBNull.Value);

            if (liability.RecurringDuration != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@duration", liability.RecurringDuration);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@duration", DBNull.Value);



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

            cmd.CommandText = @"SELECT * FROM UserLiability WHERE LiabilityID = @id";
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
                    liability.LiabilityType = reader.GetString(2);
                    liability.LiabilityDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null;
                    liability.Cost = reader.GetDecimal(4);
                    liability.DueDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;
                    liability.RecurringType = reader.GetString(6);
                    liability.RecurringDuration = !reader.IsDBNull(7) ? reader.GetInt32(7) : (int?)null;
                    liability.UserID = reader.GetInt32(6);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return liability;
        }

        public List<Liability> GetAllLiability(int userid)
        {
            List<Liability> liabilityList = new List<Liability>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserLiability WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", userid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    liabilityList.Add(
                    new Liability
                        {
                            LiabilityID = reader.GetInt32(0),
                            LiabilityName = reader.GetString(1),
                            LiabilityType = reader.GetString(2),
                            LiabilityDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null,
                            Cost = reader.GetDecimal(4),
                            DueDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                            RecurringType = reader.GetString(6),
                            RecurringDuration = !reader.IsDBNull(7) ? reader.GetInt32(7) : (int?)null,
                            UserID = userid,
                        }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return liabilityList;
        }
    }
}
