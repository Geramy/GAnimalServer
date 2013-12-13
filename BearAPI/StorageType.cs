using System;
using System.Collections.Generic;
using System.Text;

namespace BearAPI
{
    public class StorageType
    {
        public MysqlPlugin MP = new MysqlPlugin();
        public UUID sID;
        public StorageType(UUID ID)
        {
            sID = ID;
        }
        public StorageType(string OwnerName, string OwnerKey, string StorageType, string amount, string Loss)
        {
            sID = MP.NewStorageUnit(OwnerName, OwnerKey, StorageType, amount, Loss);
        }
        public string SType
        {
            get
            {
                return MP.GetStorageType(sID);
            }
        }
        public string Loss
        {
            get
            {
                return MP.GetStorageLoss(sID);
            }
        }
        public string Amount
        {
            get
            {
                return MP.GetAmount(sID);
            }
            set
            {
                MP.SetAmount(sID, value);
            }
        }
    }
}
