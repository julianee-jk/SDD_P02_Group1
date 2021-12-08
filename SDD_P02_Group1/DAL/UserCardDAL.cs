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
    public class UserCardDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UserCardDAL()
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

        public int AddUserCard(UserCard card, int userID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated CardID after insertion
            cmd.CommandText = @"INSERT INTO UserCard(CardName, CardType, CardDesc, UserID) 
                                OUTPUT INSERTED.CardID 
                                VALUES(@name, @type, @description, @userid)";

            cmd.Parameters.AddWithValue("@name", card.CardName);
            cmd.Parameters.AddWithValue("@type", card.CardType);
            cmd.Parameters.AddWithValue("@userid", userID);

            if (card.CardDesc != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@description", card.CardDesc);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@description", DBNull.Value);

            //A connection to database must be opened before any operations made.
            conn.Open();
            card.CardID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return card.CardID;
        }

        public UserCard GetUserCardDetails(int cardid)
        {
            UserCard card = new UserCard();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserCard WHERE CardID = @cardid";
            cmd.Parameters.AddWithValue("@cardid", cardid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    card.CardID = cardid;
                    card.CardName = reader.GetString(1);
                    card.CardType = reader.GetString(2);
                    card.CardDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null;
                    card.UserID = reader.GetInt32(4);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return card;
        }

        public List<UserCard> GetAllUserCard (int userid)
        {
            List<UserCard> userCardList = new List<UserCard>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserCard WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", userid);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userCardList.Add(
                    new UserCard
                        {
                            CardID = reader.GetInt32(0),
                            CardName = reader.GetString(1),
                            CardType = reader.GetString(2),
                            CardDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null,
                            UserID = userid,
                        }
                    );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return userCardList;
        }
    }
}
