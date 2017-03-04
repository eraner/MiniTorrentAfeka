﻿using System;
using System.ServiceModel;
using MiniTorrent_MediationServerContract;

namespace WcfMediationService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(MediationService)))
            {
                host.Open();
                Console.WriteLine("Mediation server strated...");
                Console.ReadLine();
            }
        }

    }
}
