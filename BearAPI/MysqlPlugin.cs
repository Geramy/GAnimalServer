using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Net;

namespace BearAPI
{
    public class MysqlPlugin
    {
        private string MyConString = "" +
            "SERVER=ecolife.postaddit.com;" +
            "DataBase=ecolifebears;" +
            "Uid=ecolife;" +
            "Pwd=latitude;";
        public MysqlPlugin()
        {
        }

        #region BearStatus_Table
        public UUID NewBear(string OwnerName, string OwnerKey, string Breed, string type, string Region, string Position, string Rotation)
        {/* OwnerName=Zayne%20Exonar&OwnerKey=faaf0c17%2D1432%2D4886%2Da17f%2D5e286324943b&Breed=&type=Grizzly%20Bear&Region=Gigli%20Portico&Position=%3C24%2E17173%2C%2037%2E28614%2C%2034%2E46193%3E&Rotation=%3C0%2E00000%2C%200%2E00000%2C%200%2E00000%2C%201%2E00000%3E&*/
            UUID ID = new UUID();
            Random r = new Random();
            Vector3D v3D = new Vector3D(Position);
            ID.GenerateUUID("BearStatus");
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO BearStatus " +
            "(`BearID`,`Hunger`,`Thirst`,`Fun`,`Health`,`Depression`,`BearName`,`Age`,`Breed`,`Gender`,`Type`,`serveraddress`)" +
            "VALUES ('"+ID.UUIDs+"', '50', '50', '50', '100', '0', 'EcoLife-Bear', '0', '"+Breed+"', '"+ (r.Next(1, 2) == 2 ? "Male" : "Female") +"', '"+type+"', 'NULL')", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;

                    mySqlCommand.ExecuteNonQuery();

                }
                mySqlConnection.Close();
            }

            //command.CommandText = 
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO BearLocation " +
            "(`BearID`,`Region`,`position`,`rotation`,`slurl`)" +
            "VALUES ('" + ID.UUIDs + "','" + Region + "', '" + Position + "', '" + Rotation + "', 'http://slurl.com/secondlife/"
            + Region + "/" + v3D.X + "/" + v3D.Y + "/" + v3D.Z + "')", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }

            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `BearOwner` " +
            "(`BearID`,`OwnerName`,`OwnerKey`)" +
            "VALUES ('" + ID.UUIDs + "', '" + OwnerName + "', '" + OwnerKey + "')", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            return ID;
        }
        public void DeleteBear(UUID ID)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("DELETE FROM `BearStatus` WHERE BearID = '" + ID.UUIDs+"'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }

            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("DELETE FROM `BearLocation` WHERE BearID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }

            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("DELETE FROM `BearOwner` WHERE BearID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public List<UUID> GetBears(string OwnerName)
        {
            List<UUID> retList = new List<UUID>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearOwner WHERE OwnerName = '" + OwnerName + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retList.Add(new UUID(Reader.GetString(Reader.GetOrdinal("BearID"))));
                        while (Reader.Read())
                        {
                            retList.Add(new UUID(Reader.GetString(Reader.GetOrdinal("BearID"))));
                        }
                    }
                }
                mySqlConnection.Close();
            }
            return retList;
        }

        public string GetStatus(UUID BearID)
        {
            string StrRet = "";
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM `BearStatus` WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        StrRet = Reader.GetString(Reader.GetOrdinal("Status"));
                    }
                    else
                    {
                        StrRet = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            
            return StrRet;
        }
        public void SetStatus(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Status = '"+value+"' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetOwnerName(UUID BearID)
        {
            string StrRet = "";
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM `BearOwner` WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        StrRet = Reader.GetString(Reader.GetOrdinal("OwnerName"));
                    }
                    else
                    {
                        StrRet = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return StrRet;
        }
        public void SetOwnerName(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearOwner` SET OwnerName = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetOwnerKey(UUID BearID)
        {
            string StrRet = "";
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM `BearOwner` WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        StrRet = Reader.GetString(Reader.GetOrdinal("OwnerKey"));
                    }
                    else
                    {
                        StrRet = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return StrRet;
        }
        public void SetOwnerKey(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearOwner` SET OwnerKey = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetHealth(UUID BearID)
        {
            //connection = new MySqlConnection(con
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        System.Data.DataTable dc = Reader.GetSchemaTable();
                        retData = Reader.GetString(Reader.GetOrdinal("Health"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetHealth(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Health = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetHunger(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        System.Data.DataTable dc = Reader.GetSchemaTable();
                        retData = Reader.GetString(Reader.GetOrdinal("Hunger"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetHunger(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Hunger = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetThirst(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        System.Data.DataTable dc = Reader.GetSchemaTable();
                        retData = Reader.GetString(Reader.GetOrdinal("Thirst"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetThirst(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Thirst = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetFun(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Fun"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetFun(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Fun = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetDepression(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Depression"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetDepression(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Depression = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetAge(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Age"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetAge(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET OwnerName = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetName(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("BearName"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetName(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET BearName = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetGender(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Gender"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetGender(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Gender = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetType(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearStatus WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Type"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetType(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearStatus` SET Type = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }
        #endregion
        #region BearLocation
        public string GetPosition(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearLocation WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("position"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetPosition(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearLocation` SET position = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetRegion(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearLocation WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Region"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetRegion(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearLocation` SET Region = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }

        public string GetRotation(UUID BearID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearLocation WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("rotation"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetRotation(UUID BearID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearLocation` SET rotation = '" + value + "' WHERE BearID = '" + BearID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }
        #endregion
        #region StorageUnit
        public UUID NewStorageUnit(string OwnerName, string OwnerKey, string StorageType, string amount, string Loss)
        {
            UUID ID = new UUID();
            ID.GenerateUUID("BearFood");
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `BearFood` " +
            "(`StorageID`,`OwnerName`,`OwnerKey`, `StorageType`, `Amount`, `Loss`)" +
            "VALUES ('" + ID.UUIDs + "', '" + OwnerName + "', '" + OwnerKey + "', '" + StorageType + "', '" + amount + "', '" + Loss + "')", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }

            return ID;
        }
        public void DeleteStorageUnit(UUID ID)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("DELETE FROM `BearFood` WHERE StorageID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }//UPDATE

        public string GetStorageType(UUID ID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearFood WHERE StorageID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("StorageType"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }

        public string GetStorageLoss(UUID ID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearFood WHERE StorageID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Loss"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }

        public List<UUID> GetStorageUnits(string OwnerName)
        {
            List<UUID> retList = new List<UUID>();
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearFood WHERE OwnerName = '" + OwnerName + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retList.Add(new UUID(Reader.GetString(Reader.GetOrdinal("StorageID"))));
                        while (Reader.Read())
                        {
                            retList.Add(new UUID(Reader.GetString(Reader.GetOrdinal("StorageID"))));
                        }
                        //System.Data.DataTable dc = Reader.GetSchemaTable();
                        //retData = Reader.GetString(dc.Columns["StorageType"].Ordinal);
                    }
                    else
                    {
                        //retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retList;
        }

        public string GetAmount(UUID ID)
        {
            string retData;
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("SELECT * FROM BearFood WHERE StorageID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    MySqlDataReader Reader = mySqlCommand.ExecuteReader();
                    if (Reader.Read() == true)
                    {
                        retData = Reader.GetString(Reader.GetOrdinal("Amount"));
                    }
                    else
                    {
                        retData = "ERROR:BearHack";
                    }
                    Reader.Dispose();
                }
                mySqlConnection.Close();
            }
            return retData;
        }
        public void SetAmount(UUID ID, string value)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(MyConString))
            {
                mySqlConnection.Open();
                using (MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `BearFood` SET Amount = " + value + " WHERE StorageID = '" + ID.UUIDs + "'", mySqlConnection))
                {
                    mySqlCommand.CommandType = System.Data.CommandType.Text;
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
        }
        #endregion
    }
}
