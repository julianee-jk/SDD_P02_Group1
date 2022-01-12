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

        public UserCard GetUserCardDetails(int userid, int cardid)
        {
            UserCard usercard = new UserCard();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM UserCard 
                                WHERE UserID = @selectedUserID AND CardID = @selectedCardID";

            //parameter is retrieved from the method parameter “userid”.
            cmd.Parameters.AddWithValue("@selectedUserID", userid);
            cmd.Parameters.AddWithValue("@selectedCardID", cardid);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill judge object with values from the data reader
                    usercard.CardID = cardid;
                    usercard.CardName = reader.GetString(1);
                    usercard.CardType = reader.GetString(2);
                    usercard.CardDesc = reader.GetString(3);

                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return usercard;
        }

        public UserCardSpending GetUserCardSpendingsDetails(int userid, int cardid)
        {
            List<UserCardSpending> userCardSpendingsList = new List<UserCardSpending>();
            UserCardSpending usercardspending = new UserCardSpending();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM UserCardSpending 
                                WHERE CardID = 
                                (SELECT CardID FROM UserCard WHERE UserID = @selectedUserID AND CardID = @selectedCardID)";

            //parameter is retrieved from the method parameter “userid”.
            cmd.Parameters.AddWithValue("@selectedUserID", userid);
            cmd.Parameters.AddWithValue("@selectedCardID", cardid);
            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    usercardspending.CardSpendingID = reader.GetInt32(0);
                    usercardspending.DateOfTransaction = reader.GetDateTime(1);
                    usercardspending.AmountSpent = reader.GetDecimal(2);

                    userCardSpendingsList.Add(new UserCardSpending
                    {
                        AmountSpent = usercardspending.AmountSpent
                    });

                    foreach (UserCardSpending s in userCardSpendingsList)
                    {
                        usercardspending.TotalCardAmountSpent += s.AmountSpent; // TO:DO
                    }
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return usercardspending;
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
