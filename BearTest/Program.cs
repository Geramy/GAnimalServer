using System;
using System.Collections.Generic;
using System.Text;
using BearAPI;

namespace BearTest
{
    class Program
    {
        static Bear BServer;
        static void Main(string[] args)
        {
            try
            {
                BServer = new Bear();
                Console.ReadLine();
            }
            catch (Exception e)
            {

            }
            Console.ReadLine();
            //Console.Read();
        }
    }
}
