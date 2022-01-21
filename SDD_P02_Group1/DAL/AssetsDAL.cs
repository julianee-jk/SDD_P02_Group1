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
            cmd.CommandText = @"INSERT INTO UserAsset (AssetName, AssetType, AssetDesc, InitialValue, CurrentValue, PredictedValue, UserID) OUTPUT INSERTED.AssetID VALUES (@assetname, @assettype, @desc, @initial, @current, @predicted, @userid)";
            cmd.Parameters.AddWithValue("@assetname", asset.AssetName);
            cmd.Parameters.AddWithValue("@assettype", asset.AssetType);
            cmd.Parameters.AddWithValue("@initial", asset.InitialValue);
            cmd.Parameters.AddWithValue("@current", asset.CurrentValue);
            cmd.Parameters.AddWithValue("@userid", userID);

            if (asset.PredictedValue != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@predicted", asset.PredictedValue);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@predicted", DBNull.Value);

            if (asset.AssetDesc != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@desc", asset.AssetDesc);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@desc", DBNull.Value);

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
                    asset.AssetType = reader.GetString(2);
                    asset.AssetDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null;
                    asset.InitialValue = reader.GetDecimal(4);
                    asset.CurrentValue = reader.GetDecimal(5);
                    asset.PredictedValue = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null;
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
                            AssetType = reader.GetString(2),
                            AssetDesc = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null,
                            InitialValue = reader.GetDecimal(4),
                            CurrentValue = reader.GetDecimal(5),
                            PredictedValue = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null,
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

        public int EditAsset(Asset asset, int id)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE UserAsset SET AssetName=@name,  AssetType=@type, AssetDesc=@desc, InitialValue=@initial, CurrentValue=@current, PredictedValue=@predicted WHERE AssetID = @id";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", asset.AssetName);
            cmd.Parameters.AddWithValue("@type", asset.AssetType);
            cmd.Parameters.AddWithValue("@initial", asset.InitialValue);
            cmd.Parameters.AddWithValue("@current", asset.CurrentValue);
            cmd.Parameters.AddWithValue("@id", id);

            if (asset.PredictedValue != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@predicted", asset.PredictedValue);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@predicted", DBNull.Value);

            if (asset.AssetDesc != null)
                // A branch is assigned
                cmd.Parameters.AddWithValue("@desc", asset.AssetDesc);
            else // No branch is assigned
                cmd.Parameters.AddWithValue("@desc", DBNull.Value);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }

        public void AddChange(int userid, Asset a1, Asset a2)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.Parameters.AddWithValue("@userid", userid);

            cmd.Parameters.AddWithValue("@assetid", a1.AssetID);

            if (a1.AssetType == a2.AssetType)
            {
                cmd.Parameters.AddWithValue("@assettype", a1.AssetType);
                cmd.Parameters.AddWithValue("@assettypen", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@assettype", a1.AssetType);
                cmd.Parameters.AddWithValue("@assettypen", DBNull.Value);
            }

            if (a1.AssetDesc == a2.AssetDesc)
            {
                cmd.Parameters.AddWithValue("@assetdesc", a1.AssetDesc);
                cmd.Parameters.AddWithValue("@assetdescn", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@assetdesc", a1.AssetDesc);
                cmd.Parameters.AddWithValue("@assetdescn", a2.AssetDesc);
            }

            if (a1.CurrentValue == a2.CurrentValue)
            {
                cmd.Parameters.AddWithValue("@currentvalue", a1.CurrentValue);
                cmd.Parameters.AddWithValue("@currentvaluen", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@currentvalue", a1.CurrentValue);
                cmd.Parameters.AddWithValue("@currentvaluen", a2.CurrentValue);
            }

            if (a1.CurrentValue != a2.CurrentValue || a1.AssetDesc != a2.AssetDesc || a1.AssetType != a2.AssetType)
            {
                cmd.CommandText = @"DECLARE @Time AS DATETIME = GETDATE();  
                INSERT INTO 
                    AssetChanges 
                        (UserID, AssetID, Timestamp, 
                        AssetType, AssetTypeNew, 
                        AssetDesc, AssetDescNew,     
                        CurrentValue, CurrentValueNew)
                    OUTPUT INSERTED.AssetID VALUES 
                        (@userid, @assetid, @Time, 
                        @assettype, @assettypen, 
                        @assetdesc, @assetdescn, 
                        @currentvalue, @currentvaluen)";

                //A connection to database must be opened before any operations made.
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                conn.Close();
            }
        }

        //005
        public List<AssetHistory> GetChanges(int userid)
        {
            List<AssetHistory> AHL = new List<AssetHistory>();

            SqlCommand cmd2 = conn.CreateCommand();

            cmd2.Parameters.AddWithValue("@userid", userid);

            cmd2.CommandText = @"SELECT * FROM AssetChanges WHERE UserID = @userid";

            conn.Open();

            SqlDataReader reader = cmd2.ExecuteReader();

            while (reader.Read())
            {
                AHL.Add(
                new AssetHistory
                {
                    UserID = reader.GetInt32(0),
                    AssetID = reader.GetInt32(1),
                    Timestamp = Convert.ToDateTime(reader.GetString(2)),
                    AssetType = reader.GetString(3),
                    AssetTypeNew = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null,
                    AssetDesc = reader.GetString(5),
                    AssetDescNew = !reader.IsDBNull(6) ? reader.GetString(6) : (string?)null,
                    CurrentValue = reader.GetDecimal(7),
                    CurrentValueNew = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null
                }
                );
            }

            reader.Close();

            conn.Close();

            return AHL;
        }

        //public AssetHistory GetChanges(int userid)
        //{
        //    List<AssetHistory> AHL = new List<AssetHistory>();

        //    SqlCommand cmd2 = conn.CreateCommand();

        //    cmd2.Parameters.AddWithValue("@userid", userid);

        //    cmd2.CommandText = @"SELECT * FROM AssetChanges WHERE UserID = @userid";

        //    conn.Open();

        //    SqlDataReader reader = cmd2.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        AHL.Add(
        //        new AssetHistory
        //        {
        //            UserID = reader.GetInt32(0),
        //            AssetID = reader.GetInt32(1),
        //            Timestamp = Convert.ToDateTime(reader.GetString(2)),
        //            AssetType = reader.GetString(3),
        //            AssetTypeNew = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null,
        //            AssetDesc = reader.GetString(5),
        //            AssetDescNew = !reader.IsDBNull(6) ? reader.GetString(6) : (string?)null,
        //            CurrentValue = reader.GetDecimal(7),
        //            CurrentValueNew = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null
        //        }
        //        );
        //    }

        //    reader.Close();

        //    conn.Close();

        //    return AHL[0];
        //}

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
