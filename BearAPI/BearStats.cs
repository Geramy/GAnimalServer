using System;
using System.Collections.Generic;
using System.Text;
//using System.Threading;
using System.Threading;
namespace BearAPI
{
    public class BearStats
    {
        public UUID __BS;

        public int dt;
        private Timer ProcessThread;
        private int Clock = 0;
        private float run_throughs = 0.0f;
        private List<UUID> B_And_S = new List<UUID>();
        private MysqlPlugin SQL = new MysqlPlugin();
        public BearStorage _BS;
        public List<StorageType> _ST = new List<StorageType>();

        public BearStats(BearStorage BS, int clock_minutes)
        {
            dt = DateTime.Today.Day;
            run_throughs = 0.0f;
            Clock = clock_minutes;
            __BS = BS.BearUUID;
            _BS = BS;
            ProcessThread = new Timer(new TimerCallback(ProcessThread_Elapsed), (object)BS.BearUUID.UUIDs, 120000, clock_minutes * 60 * 1000);
            _BS.Status = BearStorage.Stat.Online;
        }

        int fun = -1, hunger = -1, thirst = -1;
        void ProcessThread_Elapsed(object sender)
        {
            try
            {
                BearStorage N_BS = new BearStorage(new UUID((string)sender));
                if (DateTime.Today.Day != dt && DateTime.Today.Day < dt || DateTime.Today.Day > dt)
                {
                    dt = DateTime.Today.Day;
                    N_BS.Age = Convert.ToString((Convert.ToInt32(N_BS.Age) + 1));
                }
                List<UUID> Temp = SQL.GetStorageUnits(SQL.GetOwnerName(N_BS.BearUUID));
                List<StorageType> temp = new List<StorageType>();
                _ST.Clear();
                foreach (UUID Uid in Temp)
                {
                    StorageType ST = new StorageType(Uid);
                    if (Convert.ToInt32(ST.Amount) <= 0) SQL.DeleteStorageUnit(Uid);
                    else
                        _ST.Add(ST);
                }
                int i = 0;
                foreach (StorageType st in _ST)
                {
                    if (st.SType.ToLower() == "fun" && Convert.ToInt32(st.Amount) > 0) fun = i;
                    if (st.SType.ToLower() == "hunger" && Convert.ToInt32(st.Amount) > 0) hunger = i;
                    if (st.SType.ToLower() == "thirst" && Convert.ToInt32(st.Amount) > 0) thirst = i;
                    i++;
                }
                //if (Convert.ToInt32(_ST[fun].Amount) < 0 && run_throughs == 0.0f || Convert.ToInt32(_ST[hunger].Amount) < 0 && run_throughs == 0.5f || Convert.ToInt32(_ST[thirst].Amount) < 0 && run_throughs == 1.0f) return;
                Random r = new Random();
                System.Threading.Thread.Sleep(r.Next(0, 10));
                Console.WriteLine(N_BS.BearUUID.UUIDs + "Time:" + (run_throughs == 0.0f ? "Fun" : run_throughs == 0.05f ? "Hunger" : "Thirst"));
                if (run_throughs == 0.0f)//fun
                {
                    if (fun != -1)
                    {
                        if (Convert.ToInt32(N_BS.Fun) + 2 <= 100 && Convert.ToInt32(_ST[fun].Amount) > 0)
                        {
                            N_BS.Fun = Convert.ToString(Convert.ToInt32(N_BS.Fun) + 2);
                            _ST[fun].Amount = Convert.ToString(Convert.ToInt32(_ST[fun].Amount) - Convert.ToInt32(_ST[fun].Loss));
                        }
                        else if (Convert.ToInt32(N_BS.Fun) + 2 <= 100 && Convert.ToInt32(_ST[fun].Amount) < 0)
                        {
                            N_BS.Fun = Convert.ToString(Convert.ToInt32(N_BS.Fun) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Fun) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) <= 100 && Convert.ToInt32(N_BS.Health) - 2 >= 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Fun) - 1 >= 0)
                        {
                            N_BS.Fun = Convert.ToString(Convert.ToInt32(N_BS.Fun) - 1);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(N_BS.Fun) <= 100 && Convert.ToInt32(N_BS.Fun) > 0)
                        {
                            N_BS.Fun = Convert.ToString(Convert.ToInt32(N_BS.Fun) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Fun) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) >= 100 && Convert.ToInt32(N_BS.Health) > 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Fun) > 0)
                        {
                            N_BS.Fun = Convert.ToString(Convert.ToInt32(N_BS.Fun) - 1);
                        }
                    }
                    run_throughs += 0.5f;
                }
                else if (run_throughs == 0.5f)//hunger
                {
                    if (hunger != -1)
                    {
                        if (Convert.ToInt32(N_BS.Hunger) + 2 <= 100 && Convert.ToInt32(_ST[hunger].Amount) > 0)
                        {
                            N_BS.Hunger = Convert.ToString(Convert.ToInt32(N_BS.Hunger) + 2);
                            _ST[hunger].Amount = Convert.ToString(Convert.ToInt32(_ST[hunger].Amount) - Convert.ToInt32(_ST[hunger].Loss));
                        }
                        else if (Convert.ToInt32(N_BS.Hunger) + 2 <= 100 && Convert.ToInt32(_ST[hunger].Amount) < 0)
                        {
                            N_BS.Hunger = Convert.ToString(Convert.ToInt32(N_BS.Hunger) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Hunger) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) <= 100 && Convert.ToInt32(N_BS.Health) - 2 >= 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Hunger) - 1 >= 0)
                        {
                            N_BS.Hunger = Convert.ToString(Convert.ToInt32(N_BS.Hunger) - 1);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(N_BS.Hunger) <= 100 && Convert.ToInt32(N_BS.Hunger) > 0)
                        {
                            N_BS.Hunger = Convert.ToString(Convert.ToInt32(N_BS.Hunger) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Hunger) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) >= 100 && Convert.ToInt32(N_BS.Health) > 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Hunger) > 0)
                        {
                            N_BS.Hunger = Convert.ToString(Convert.ToInt32(N_BS.Hunger) - 1);
                        }
                    }
                    run_throughs += 0.5f;
                }
                else if (run_throughs == 1.0f)//thirst
                {
                    if (thirst != -1)
                    {
                        if (Convert.ToInt32(N_BS.Thirst) + 2 <= 100 && Convert.ToInt32(_ST[thirst].Amount) > 0)
                        {
                            N_BS.Thirst = Convert.ToString(Convert.ToInt32(N_BS.Thirst) + 2);
                            _ST[thirst].Amount = Convert.ToString(Convert.ToInt32(_ST[thirst].Amount) - Convert.ToInt32(_ST[thirst].Loss));
                        }
                        else if (Convert.ToInt32(N_BS.Thirst) + 2 <= 100 && Convert.ToInt32(_ST[thirst].Amount) < 0)
                        {
                            N_BS.Thirst = Convert.ToString(Convert.ToInt32(N_BS.Thirst) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Thirst) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) <= 100 && Convert.ToInt32(N_BS.Health) - 2 >= 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Thirst) - 1 >= 0)
                        {
                            N_BS.Thirst = Convert.ToString(Convert.ToInt32(N_BS.Thirst) - 1);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(N_BS.Thirst) <= 100 && Convert.ToInt32(N_BS.Thirst) > 0)
                        {
                            N_BS.Thirst = Convert.ToString(Convert.ToInt32(N_BS.Thirst) - 2);
                        }
                        else if (Convert.ToInt32(N_BS.Thirst) <= 0)
                        {//Gain Depression
                            if (Convert.ToInt32(N_BS.Depression) + 2 <= 100)
                            {
                                N_BS.Depression = Convert.ToString(Convert.ToInt32(N_BS.Depression) + 2);
                            }
                            else if (Convert.ToInt32(N_BS.Depression) >= 100 && Convert.ToInt32(N_BS.Health) > 0)
                            {
                                N_BS.Health = Convert.ToString(Convert.ToInt32(N_BS.Health) - 2);
                            }
                        }
                        if (Convert.ToInt32(N_BS.Thirst) > 0)
                        {
                            N_BS.Thirst = Convert.ToString(Convert.ToInt32(N_BS.Thirst) - 1);
                        }
                    }
                    run_throughs = 0.0f;
                }
                //_ST.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data + " " + e.InnerException + " " + e.Source + " " + e.Message);
            }
        }
        public string Stop()
        {
            _BS.Status = BearStorage.Stat.Offline;
            ProcessThread.Dispose();
            return __BS.UUIDs;
        }
        public string Start()
        {
            dt = DateTime.Today.Day;
            //run_throughs = 0.0f;
            //_BS.Status = BearStorage.Stat.Online;
            //ProcessThread = new Timer(new TimerCallback(ProcessThread_Elapsed), (object)__BS.UUIDs, 120000, Clock  * 60 * 1000);
            
            return __BS.UUIDs;
            
        }
    }
}
