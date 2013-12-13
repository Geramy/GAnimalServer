using System;
using System.Collections.Generic;
using System.Text;
using DynamicWebServer;

namespace BearAPI
{
    public delegate byte[] HTTPResponse(Dictionary<string, string> POST);
    public class BearsHTTP
    {
        public event HTTPResponse OnResponse;
        DynamicWebServer.SimpleWebServer SWS = new SimpleWebServer(8010);
        public BearsHTTP()
        {
            SWS.OnCommand += new SimpleWebServer.GotCommand(SWS_OnCommand);
            SWS.StartListen();
        }
        public void Start()
        {
            SWS.StartListen();
        }
        public void Stop()
        {
            SWS.EndListener();
        }

        byte[] SWS_OnCommand(string[] Commands, string[] Variables)
        {
            Dictionary<string, string> _POST = new Dictionary<string, string>();
            int i = 0;
            for(i = 0; i < Commands.Length; i++)
            {
                _POST.Add(Commands[i], Variables[i]);
            }
            return OnResponse(_POST);
        }
    }
}
