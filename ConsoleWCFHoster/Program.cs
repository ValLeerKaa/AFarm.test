using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfServer;

namespace ConsoleWCFHoster
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost WcfServiceHost = null;
            try
            {
                //Base Address for StudentService
                Uri httpBaseAddress = new Uri("http://localhost:4321/WcfServer");

                //Instantiate ServiceHost
                using (WcfServiceHost = new ServiceHost(typeof(WcfServer.WcfServer),
                    httpBaseAddress))
                {

                    //Add Endpoint to Host
                    WcfServiceHost.AddServiceEndpoint(typeof(IContract),
                                                            new BasicHttpBinding(), "");

                    //Metadata Exchange
                    ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                    serviceBehavior.HttpGetEnabled = true;
                    WcfServiceHost.Description.Behaviors.Add(serviceBehavior);

                    //Open
                    WcfServiceHost.Open();
                    Console.WriteLine($"Service is alive now at: {httpBaseAddress}, press Enter to close." );
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                WcfServiceHost = null;
                Console.WriteLine("There is an issue with Service:\n" + ex.Message);
                if (WcfServiceHost!=null) WcfServiceHost.Close();
            }

        }
    }
}
