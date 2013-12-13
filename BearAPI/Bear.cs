using System;
using System.Collections.Generic;
using System.Text;
using DynamicWebServer;
using System.Text.RegularExpressions;

namespace BearAPI
{
    public class Bear
    {
        static internal MysqlPlugin SQL = new MysqlPlugin();
        static internal Dictionary<string, BearStats> BSUUID = new Dictionary<string, BearStats>();
        static internal Dictionary<string, StorageType> STUUID = new Dictionary<string, StorageType>();
        static internal Dictionary<string, BearStorage> ActiveBears = new Dictionary<string, BearStorage>();

        static internal DynamicWebServer.SimpleWebServer SWS = new SimpleWebServer(8010);
        public const int Status_time = 1;
        public Bear()
        {
            SWS.OnCommand += new SimpleWebServer.GotCommand(HTTP_OnResponse);
        }
        static internal void Start()
        {
            SWS.StartListen();
        }
        static internal void Stop()
        {
            SWS.EndListener();
        }
        static internal bool BearCheck(UUID ID)
        {
            bool RetData = true;
            string key = ID.UUIDs;
            if (BSUUID.ContainsKey(key))
            {
                RetData = (new BearStorage(ID).Status == BearStorage.Stat.Online ? false : true);
                string var = (new BearStorage(ID).Status == BearStorage.Stat.Online ? "" : BSUUID[key].Start());

            }
            else
            {
                ActiveBears.Add(ID.UUIDs, new BearStorage(ID));
                BSUUID.Add(key, new BearStats(ActiveBears[key], Status_time));
                BSUUID[key].Start();
                RetData = true;
            }
            return RetData;
        }
        byte[] HTTP_OnResponse(string[] Commands, string[] Variables)
        {
            string RetData = "";
            try
            {
            //System.Threading.Thread.Sleep(1);
                Dictionary<string, string> POST = new Dictionary<string, string>();
                int i = 0;
                for (i = 0; i < Commands.Length; i++)
                {
                    POST.Add(Commands[i], Variables[i]);
                }

                //System.Threading.Thread.Sleep(5);

                #region MainComs
                //Command <type> 
                //Value <data>
                if (POST["Command"] == "NewBear")
                {//string OwnerName, string OwnerKey, string Breed, string type, string Region, string Position, string Rotation
                    BearStorage __BS = new BearStorage(POST["OwnerName"], POST["OwnerKey"], POST["Breed"], POST["type"], POST["Region"], POST["Position"], POST["Rotation"]);
                    BSUUID.Add(__BS.BearUUID.UUIDs, new BearStats(__BS, Status_time));
                    RetData = __BS.BearUUID.UUIDs;
                }
                else if (POST["Command"] == "DeleteBear")
                {
                    BSUUID[Regex.Replace(POST["UUID"], @"\s", "" )].Stop();
                    SQL.DeleteBear(new UUID(Regex.Replace(POST["UUID"], @"\s", "" )));
                    BSUUID.Remove(Regex.Replace(POST["UUID"], @"\s", "" ));
                    RetData = "DELETED";
                }

                else if (POST["Command"] == "GetBears")
                {
                    RetData = "";
                    List<UUID> Uid = SQL.GetBears(POST["OwnerName"]);
                    foreach (UUID id in Uid)
                    {
                        RetData += id.UUIDs + "~";
                    }
                }

                else if (POST["Command"] == "StopBear")
                {
                    string key = Regex.Replace(POST["UUID"], @"\s", "");
                    if (BSUUID.ContainsKey(key))
                    {

                        RetData = (new BearStorage(new UUID(key)).Status == BearStorage.Stat.Online ? "Stopped" : "AlreadyStopped");
                        string var = (new BearStorage(new UUID(key)).Status == BearStorage.Stat.Online ? BSUUID[key].Stop() : "");
                    }
                    else
                    {
                        ActiveBears.Remove(key);
                        BSUUID[key].Stop();
                        BSUUID.Remove(key);
                        RetData = "NotStarted";
                    }

                }
                else if (POST["Command"] == "StartBear")
                {
                    string key = Regex.Replace(POST["UUID"], @"\s", "");
                    if (BSUUID.ContainsKey(key))
                    {
                        RetData = (new BearStorage(new UUID(key)).Status == BearStorage.Stat.Online ? "AlreadyStarted" : "Started");
                        string var = (new BearStorage(new UUID(key)).Status == BearStorage.Stat.Online ? "" : BSUUID[key].Start());

                    }
                    else
                    {
                        ActiveBears.Add(Regex.Replace(POST["UUID"], @"\s", ""), new BearStorage(new UUID(Regex.Replace(POST["UUID"], @"\s", ""))));
                        BSUUID.Add(key, new BearStats(ActiveBears[key], 5));
                        //BSUUID[key].Start();
                        RetData = "NewInstance";
                    }
                }
                #endregion
                #region BearStorage_GET_Coms
                else if (POST["Command"] == "GetHealth")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Health;
                }
                else if (POST["Command"] == "GetHunger")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Hunger;
                }
                else if (POST["Command"] == "GetThirst")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Thirst;
                }
                else if (POST["Command"] == "GetFun")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Fun;
                }
                else if (POST["Command"] == "GetDepression")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Depression;
                }
                else if (POST["Command"] == "GetName")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Name;
                }
                else if (POST["Command"] == "GetAge")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Age;
                }
                else if (POST["Command"] == "GetGender")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Gender;
                }
                else if (POST["Command"] == "GetRegion")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Region;
                }
                else if (POST["Command"] == "GetPosition")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Position.Vector;
                }
                else if (POST["Command"] == "GetRotation")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Rotation;
                }
                else if (POST["Command"] == "GetOwnerName")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = SQL.GetOwnerName(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                else if (POST["Command"] == "GetOwnerKey")
                {
                    BearCheck(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = SQL.GetOwnerKey(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                else if (POST["Command"] == "GetBears")
                {
                    foreach (UUID Uid in SQL.GetBears(POST["OwnerName"])) { RetData += Uid.UUIDs + "~"; }
                }
                #endregion
                #region BearStorage_SET_Coms
                else if (POST["Command"] == "SetHealth")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Health = POST["Health"];
                }
                else if (POST["Command"] == "SetHunger")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Hunger = POST["Hunger"];
                }
                else if (POST["Command"] == "SetThirst")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Thirst = POST["Thirst"];
                }
                else if (POST["Command"] == "SetFun")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Fun = POST["Fun"];
                }
                else if (POST["Command"] == "SetDepression")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Depression = POST["Depression"];
                }
                else if (POST["Command"] == "SetName")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Name = POST["Name"];
                }
                else if (POST["Command"] == "SetGender")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Gender = POST["Gender"]; ;
                }
                else if (POST["Command"] == "SetAge")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Age = POST["Age"];
                }

                else if (POST["Command"] == "SetRegion")
                {
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Region = POST["Region"];
                }
                else if (POST["Command"] == "SetPosition")
                {
                    BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Position = new Vector3D(POST["Position"]);
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Position.Vector;
                }
                else if (POST["Command"] == "SetRotation")
                {
                    BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Rotation = POST["Rotation"];
                    RetData = BSUUID[Regex.Replace(POST["UUID"], @"\s", "")]._BS.Rotation;
                }

                else if (POST["Command"] == "SetOwnerName")
                {
                    SQL.SetOwnerName(new UUID(Regex.Replace(POST["UUID"], @"\s", "")), POST["OwnerName"]);
                    RetData = SQL.GetOwnerName(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                else if (POST["Command"] == "SetOwnerKey")
                {
                    SQL.SetOwnerKey(new UUID(Regex.Replace(POST["UUID"], @"\s", "")), POST["OwnerKey"]);
                    RetData = SQL.GetOwnerKey(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                #endregion
                #region StorageType
                else if (POST["Command"] == "NewStorageType")
                {
                    StorageType ST = new StorageType(POST["OwnerName"], POST["OwnerKey"], POST["StorageType"], POST["amount"], POST["Loss"]);
                    RetData = ST.sID.UUIDs;
                }
                else if (POST["Command"] == "DeleteStorageType")
                {
                    SQL.DeleteStorageUnit(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                    RetData = "DELETED";
                }
                else if (POST["Command"] == "GetAmount")
                {
                    RetData = SQL.GetAmount(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                else if (POST["Command"] == "SetAmount")
                {
                    SQL.SetAmount(new UUID(Regex.Replace(POST["UUID"], @"\s", "")), POST["Amount"]);
                    RetData = SQL.GetAmount(new UUID(Regex.Replace(POST["UUID"], @"\s", "")));
                }
                #endregion
            }
            catch (Exception e)
            {
                //SQL.CloseConnection(ref SQL);
                RetData = e.Message;
                Console.WriteLine("An Error Occured " + e.Message);
            }
            //System.Threading.Thread.Sleep(5);
            return ASCIIEncoding.ASCII.GetBytes(RetData);
        }
    }
}
/*
the ai database

-- phpMyAdmin SQL Dump
-- version 3.3.10
-- http://www.phpmyadmin.net
--
-- Host: ecolife.postaddit.com
-- Generation Time: Aug 30, 2011 at 06:55 PM
-- Server version: 5.1.53
-- PHP Version: 5.2.17

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";

--
-- Database: `ecolifebears`
--

-- --------------------------------------------------------

--
-- Table structure for table `BearFood`
--

CREATE TABLE IF NOT EXISTS `BearFood` (
  `StorageID` varchar(254) NOT NULL,
  `OwnerName` varchar(254) NOT NULL,
  `OwnerKey` varchar(254) NOT NULL,
  `StorageType` varchar(254) NOT NULL,
  `Amount` varchar(254) NOT NULL,
  `Loss` varchar(254) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `BearLocation`
--

CREATE TABLE IF NOT EXISTS `BearLocation` (
  `BearID` varchar(254) NOT NULL,
  `Region` varchar(254) NOT NULL,
  `position` varchar(254) NOT NULL,
  `rotation` varchar(254) NOT NULL,
  `slurl` varchar(254) NOT NULL,
  UNIQUE KEY `BearID` (`BearID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `BearOwner`
--

CREATE TABLE IF NOT EXISTS `BearOwner` (
  `BearID` varchar(254) NOT NULL,
  `OwnerName` varchar(254) NOT NULL,
  `OwnerKey` varchar(254) NOT NULL,
  UNIQUE KEY `BearID` (`BearID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `BearStatus`
--

CREATE TABLE IF NOT EXISTS `BearStatus` (
  `BearID` varchar(254) NOT NULL,
  `Hunger` varchar(254) NOT NULL,
  `Thirst` varchar(254) NOT NULL,
  `Fun` varchar(254) NOT NULL,
  `Health` varchar(254) NOT NULL,
  `Depression` varchar(254) NOT NULL,
  `BearName` varchar(254) NOT NULL,
  `Age` varchar(254) NOT NULL,
  `Breed` varchar(254) NOT NULL,
  `Gender` varchar(254) NOT NULL,
  `Type` varchar(254) NOT NULL,
  `serveraddress` varchar(254) NOT NULL,
  `Status` varchar(254) NOT NULL,
  UNIQUE KEY `BearID` (`BearID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE IF NOT EXISTS `Users` (
  `username` varchar(254) NOT NULL,
  `password` varchar(254) NOT NULL,
  `money` varchar(254) NOT NULL,
  UNIQUE KEY `username` (`username`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

*/