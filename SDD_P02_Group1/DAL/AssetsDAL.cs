using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SDD_P02_Group1.Models;

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

        public int AddAsset(Asset asset, int userID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO UserAsset (AssetName, InitialValue, CurrentValue, PredictedValue) OUTPUT INSERTED.AssetID VALUES (@assetname, @initial, @current, @predicted, @userid)";
            cmd.Parameters.AddWithValue("@assetname", asset.AssetName);
            cmd.Parameters.AddWithValue("@initial", asset.InitialValue);
            cmd.Parameters.AddWithValue("@current", asset.CurrentValue);
            cmd.Parameters.AddWithValue("@userid", userID);


            if (asset.PredictedValue != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@predicted", asset.PredictedValue);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@predicted", DBNull.Value);

            //A connection to database must be opened before any operations made.
            conn.Open();
            asset.AssetID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return asset.AssetID;
        }



        public Asset GetAssetDetails(int assetID)
        {
            Asset asset = new Asset();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserAsset WHERE AssetID = @id";
            cmd.Parameters.AddWithValue("@id", assetID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    asset.AssetID = assetID;
                    asset.AssetName = reader.GetString(1);
                    asset.InitialValue = reader.GetDecimal(2);
                    asset.CurrentValue = reader.GetDecimal(3);
                    asset.PredictedValue = !reader.IsDBNull(4) ? reader.GetDecimal(4) : (decimal?)null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return asset;
        }


        public List<Asset> GetAllAsset(int userID)
        {
            List<Asset> assetList = new List<Asset>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM UserAsset WHERE UserID = @id";
            cmd.Parameters.AddWithValue("@id", userID);

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
                            AssetID = reader.GetInt32(0),
                            AssetName = reader.GetString(1),
                            InitialValue = reader.GetDecimal(2),
                            CurrentValue = reader.GetDecimal(3),
                            PredictedValue = !reader.IsDBNull(4) ? reader.GetDecimal(4) : (decimal?)null,
                            UserID = userID,
                        }
                    );;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return assetList;
        }

        public int EditAsset(Asset asset)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE UserAsset SET AssetName=@name, InitialValue=@initial, CurrentValue=@current, PredictedValue=@predicted WHERE AssetID = @id";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", asset.AssetName);
            cmd.Parameters.AddWithValue("@initial", asset.InitialValue);
            cmd.Parameters.AddWithValue("@current", asset.CurrentValue);
            cmd.Parameters.AddWithValue("@predicted", asset.PredictedValue);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public int DeleteAsset(int assetID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"DELETE FROM UserAsset WHERE AssetID = @id";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@id", assetID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int rowAffected = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return rowAffected;
        }
    }
}
