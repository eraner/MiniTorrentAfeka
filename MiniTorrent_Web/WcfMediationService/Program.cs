using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MiniTorrent_MediationServerContract;

namespace WcfMediationService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(MediationService),
                new Uri[] { new Uri("http://localhost:8089/MiniTorrentWcf/") }))
            {
                host.AddServiceEndpoint(typeof(IMediationServerContract),
                    new BasicHttpBinding(), "MediationService");

                host.Open();
                Console.WriteLine("Mediation server strated...");
                Console.ReadLine();
                host.Close();

            }
        }

    }
}
