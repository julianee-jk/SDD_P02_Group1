using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using SDD_P02_Group1.Models;

namespace SDD_P02_Group1.DAL
{
    public class UserDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UserDAL()
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

        public int ResetPassword(string email, string password)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE AccountUser SET Password=@password WHERE Email = @email";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
           
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public List<User> GetAllUsers()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM AccountUser";

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a judge list
            List<User> userList = new List<User>();

            while (reader.Read())
            {
                userList.Add(
                    new User
                    {
                        UserId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        EmailAddr = reader.GetString(3)

                    }
                );
            }

            //Close DataReader
            reader.Close();

            //Close the database connection
            conn.Close();
            return userList;
        }

        public User GetDetails(int userid)
        {
            User user = new User();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM AccountUser
                                WHERE UserID = @selectedUserID";

            //parameter is retrieved from the method parameter “userid”.
            cmd.Parameters.AddWithValue("@selectedUserID", userid);

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
                    user.UserId = userid;
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.EmailAddr = reader.GetString(3);

                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return user;
        }

        public int EditUser(User user, int id)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE AccountUser SET UserName=@name, Password=@password, Email=@email WHERE UserID = @id";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@email", user.EmailAddr);
            cmd.Parameters.AddWithValue("@id", id);


            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public int Add(User user)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated JudgeID after insertion
            cmd.CommandText = @"INSERT INTO AccountUser(Username, Password, Email) 
                                OUTPUT INSERTED.UserID
                                VALUES(@name, @password, @email)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", user.Username);
            cmd.Parameters.AddWithValue("@email", user.EmailAddr);
            cmd.Parameters.AddWithValue("@password", user.Password);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //JudgeID after executing the INSERT SQL statement
            user.UserId = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return user.UserId;
        }

        public bool IsEmailExist(string email, int userId)
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a judge record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT UserID FROM AccountUser WHERE Email=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and execute the SQL statement
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == userId)
                    {
                        break;
                    }
                    else if (reader.GetInt32(0) != userId)
                    {
                        //The email address is used by another user
                        emailFound = true;
                    }
                }
            }

            else
            { //No record
                emailFound = false; // The email address given does not exist
            }

            reader.Close();
            conn.Close();

            return emailFound;
        }

        public bool IsUserNameExist(string name, int userId)
        {
            bool nameFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a judge record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT UserID FROM AccountUser WHERE Username=@selectedName";
            cmd.Parameters.AddWithValue("@selectedName", name);

            //Open a database connection and execute the SQL statement
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == userId)
                    {
                        nameFound = false;
                    }
                    else if (reader.GetInt32(0) != userId)
                    {
                        //The name is used by another user
                        nameFound = true;
                    }

                }
            }

            else
            { //No record
                nameFound = false; // The name given does not exist
            }

            reader.Close();
            conn.Close();

            return nameFound;
        }

        public bool IsEmailExist2(string email)
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement 
            //to get a judge record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM AccountUser WHERE Email=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and execute the SQL statement
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    Console.WriteLine("lolol " + reader.GetString(3));
                    if (reader.GetString(3) == email)
                        //The email address is used by another user
                        emailFound = true;
                }
            }

            else
            { //No record
                emailFound = false; // The email address given does not exist
            }

            reader.Close();
            conn.Close();

            return emailFound;
        }
    }
}
