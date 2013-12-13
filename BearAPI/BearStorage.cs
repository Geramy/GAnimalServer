using System;
using System.Collections.Generic;
using System.Text;
using BearAPI;

namespace BearAPI
{
    public class BearStorage
    {
        public UUID BearUUID;
        private MysqlPlugin SP = new MysqlPlugin();
        public BearStorage(UUID BearsID)
        {
            BearUUID = BearsID;
        }

        public BearStorage(string OwnerName, string OwnerKey, string Breed, string type, string Region, string Position, string Rotation)
        {
            BearUUID = SP.NewBear(OwnerName, OwnerKey, Breed, type, Region, Position, Rotation);
        }

        public string Health
        {
            get
            {
                return SP.GetHealth(BearUUID);
            }
            set
            {
                SP.SetHealth(BearUUID, value);
            }
        }

        public string Hunger
        {
            get
            {
                return SP.GetHunger(BearUUID);
            }
            set
            {
                SP.SetHunger(BearUUID, value);
            }
        }

        public string Thirst
        {
            get
            {
                return SP.GetThirst(BearUUID);
            }
            set
            {
                SP.SetThirst(BearUUID, value);
            }
        }

        public string Fun
        {
            get
            {
                return SP.GetFun(BearUUID);
            }
            set
            {
                SP.SetFun(BearUUID, value);
            }
        }

        public string Depression
        {
            get
            {
                return SP.GetDepression(BearUUID);
            }
            set
            {
                SP.SetDepression(BearUUID, value);
            }
        }

        public string Name
        {
            get
            {
                return SP.GetName(BearUUID);
            }
            set
            {
                SP.SetName(BearUUID, value);
            }
        }

        public string Age
        {
            get
            {
                return SP.GetAge(BearUUID);
            }
            set
            {
                SP.SetAge(BearUUID, value);
            }
        }

        public string Region
        {
            get
            {
                return SP.GetRegion(BearUUID);
            }
            set
            {
                SP.SetRegion(BearUUID, value);
            }
        }

        public Vector3D Position
        {
            get
            {
                return new Vector3D(SP.GetPosition(BearUUID));
            }
            set
            {
                SP.SetPosition(BearUUID, value.Vector);
            }
        }

        public string Gender
        {
            get
            {
                return SP.GetGender(BearUUID);
            }
            set
            {
                SP.SetGender(BearUUID, value);
            }
        }

        public string Rotation
        {
            get
            {
                return SP.GetRotation(BearUUID);
            }
            set
            {
                SP.SetRotation(BearUUID, value);
            }
        }
        public Stat Status
        {
            get
            {
                if (SP.GetStatus(BearUUID) == "ONLINE") return Stat.Online;
                else return Stat.Offline;
            }
            set
            {
                switch (value)
                {
                    case Stat.Offline: SP.SetStatus(BearUUID, "OFFLINE");
                        break;
                    case Stat.Online: SP.SetStatus(BearUUID, "ONLINE");
                        break;
                }
            }
        }
        public enum Stat
        {
            Online,
            Offline,
        }
    }
}
