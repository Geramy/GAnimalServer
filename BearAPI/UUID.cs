using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace BearAPI
{
    public class UUID
    {
        public string UUIDs = "";
        private string MyConString = "SERVER=ecolife.postaddit.com;" +
                "DATABASE=ecolifebears;" +
                "UID=ecolife;" +
                "PASSWORD=latitude;";
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader Reader;
        public UUID()
        {
            
        }
        public UUID(string ID)
        {
            UUIDs = ID;
        }
        public UUID GenerateUUID(string Table)
        {
            string ID = "";
            bool done = false;
            connection = new MySqlConnection(MyConString);
            command = connection.CreateCommand();
            while (!done)
            {
                ID = GenUUID();
                command.CommandText = "SELECT * FROM " + Table + " WHERE " + (Table == "BearFood" ? "StorageID" : "BearID") + " = '" + ID + "'";
                connection.Open();
                Reader = command.ExecuteReader();
                if(Reader.Read() == true)
                {
                    Reader.GetString("BearsID");
                    done = false;
                }
                else
                {
                    done = true;
                }
                connection.Close();
            }
            UUIDs = ID;
            return this;
        }
        private string GenUUID() //cb5c9c5c-4922-4700-91a8-1f0a48664939
        {
            string newPassword = "";
            Random r = new Random();
            int i = 0;
            for (i = 0; i < 32; i++)
            {
                bool done = false;
                int aCharValue = 0;

                while (!done)
                {
                    aCharValue = r.Next(48, 126);
                    char aChar = Convert.ToChar(aCharValue);
                    done = false;
                    if ((aChar >= '9' && aChar <= '0')
                    || (aChar >= 'a' && aChar <= 'z'))
                        done = true;
                }

                newPassword += Convert.ToChar(aCharValue);

                if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                {
                    newPassword += "-";
                }

            }
            return newPassword;
        }
    }
}
