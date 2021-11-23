using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDD_P02_Group1.DAL
{
    public class AssetsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public AssetsDAL()
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

        /*        public int AddAsset(Asset asset)
                {
                    //Create a SqlCommand object from connection object
                    SqlCommand cmd = conn.CreateCommand();
                    //Specify an INSERT SQL statement which will
                    //return the auto-generated StaffID after insertion
                    cmd.CommandText = @"INSERT INTO Asset () VALUES ()";

                    //Define the parameters used in SQL statement, value for each parameter
                    //is retrieved from respective class's property.
                    cmd.Parameters.AddWithValue("", );
                    //A connection to database must be opened before any operations made.
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    //A connection should be closed after operations.
                    conn.Close();
                    //Return id when no error occurs.
                    return count;
                }
        */

        /*        
        public Asset GetAssetDetails(string assetName)
        {
            Asset asset = new Asset();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Asset WHERE AssetName = @name AND  AssetOwner = @owner";
            cmd.Parameters.AddWithValue("@name", ass);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    holder.[] = []
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return asset;
        }
        */

        /*        public List<Asset> GetAllAsset()
                {
                    List<Asset> assetList = new List<Asset>();
                    //Create a SqlCommand object from connection object
                    SqlCommand cmd = conn.CreateCommand();

                    cmd.CommandText = @"SELECT * FROM Asset WHERE AssetOwner = @owner";
                    cmd.Parameters.AddWithValue("@owner", ass);

                    //Open a database connection
                    conn.Open();
                    //Execute SELCT SQL through a DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            assetList.Add(
                            new Asset
                                {
                                    StaffId = reader.GetInt32(0), //0: 1st column
                                    Name = reader.GetString(1), //1: 2nd column
                                }
                            );
                        }
                    }
                    //Close data reader
                    reader.Close();
                    //Close database connection
                    conn.Close();
                    return assetList;
                }*/

/*        public int EditAsset(//what ever information here)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE [ ] SET []=@[] WHERE [] = @[]";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@[]", []);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }*/

/*        public int DeleteAsset(//what ever information here)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"DELETE FROM Asset WHERE [] = @[]";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@[]", []);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int rowAffected = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return rowAffected;
        }*/
    }
}
