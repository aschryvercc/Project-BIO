using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Service Test Harness...");
            Console.WriteLine("Starting CSV Export Service Testing...");

            CSVExportService csve = new CSVExportService();

            Console.WriteLine("Authenticating Use...");
            Console.WriteLine("Sending bad credentials...");

            string[] strs = csve.authenticate("thisis", "bad");

            Console.WriteLine("Results: ");
            Console.WriteLine("String[0] | Token String = " + strs[0] + "Expecting = GUID value");
            Console.WriteLine("String[1] | User Confirmation String = " + strs[1] + " Expecting = nvu");

            Console.WriteLine("Sending good credentials...");

            strs = csve.authenticate("username", "thisisbad");

            Console.WriteLine("Results: ");
            Console.WriteLine("String[0] | Token String = " + strs[0] + "Expecting = GUID value");
            Console.WriteLine("String[1] | User Confirmation String = " + strs[1] + " Expecting = an empty string");

            Console.WriteLine("Exporting to CSV...");
            Console.WriteLine("Sending token = " + strs[0] + "...");
            string result = csve.CSVExport(strs[0]);
            Console.WriteLine("Writing result to file...");
            Logger.logMessage(result);
            Console.WriteLine(result);

            Console.WriteLine("Starting QB Export Service Testing...");
            qbExportService qbes = new qbExportService();

            Console.WriteLine("Authenticating Use...");
            Console.WriteLine("Sending bad credentials...");

            strs = csve.authenticate("thisis", "bad");

            Console.WriteLine("Results: ");
            Console.WriteLine("String[0] | Token String = " + strs[0] + "Expecting = GUID value");
            Console.WriteLine("String[1] | User Confirmation String = " + strs[1] + " Expecting = nvu");

            Console.WriteLine("Sending good credentials...");

            strs = csve.authenticate("username", "thisisbad");

            Console.WriteLine("Results: ");
            Console.WriteLine("String[0] | Token String = " + strs[0] + "Expecting = GUID value");
            Console.WriteLine("String[1] | User Confirmation String = " + strs[1] + " Expecting = c:\\Program Files\\Intuit\\QuickBooks\\sample_product-based business.qbw");

            Console.WriteLine("Asking for requests with fake information...");
            string request = csve.sendRequestXML(strs[0], "response", "companyFileName", "qbXMLCountry", "qbXMLMajorVersion", "qbXMLMinorVersion");
            Console.WriteLine("Writing response to file...");
            Logger.logMessage(request);
            Console.WriteLine("Request = " + System.Environment.NewLine);
            Console.WriteLine(request);

            Console.WriteLine("Building Fake Respone...");
            string response = "";
            Console.WriteLine("Sending Repsonse...");
            int received = csve.receiveResponseXML(strs[0], response, "", "This is a message");

            Console.WriteLine("Wrting response to file...");
            Logger.logMessage("Percentage done = " + received.ToString());
            Console.WriteLine("Received percentage done = " + received.ToString() + " Expecting = 50");

            if (received == 50)
            {
                Console.WriteLine("Asking for requests with fake information...");
                request = csve.sendRequestXML(strs[0], "response", "companyFileName", "qbXMLCountry", "qbXMLMajorVersion", "qbXMLMinorVersion");
                Console.WriteLine("Writing response to file...");
                Logger.logMessage(request);
                Console.WriteLine("Request = " + System.Environment.NewLine);
                Console.WriteLine(request);

                Console.WriteLine("Building Fake Respone...");
                response = "";
                Console.WriteLine("Sending Repsonse...");
                received = csve.receiveResponseXML(strs[0], response, "", "This is a message");

                Console.WriteLine("Wrting response to file...");
                Logger.logMessage("Percentage done = " + received.ToString());
                Console.WriteLine("Received percentage done = " + received.ToString() + " Expecting = 100");

                if (received != 100)
                {
                    Console.WriteLine("Something went Wrong...");
                }
            }
            else
            {
                Console.WriteLine("Sending ConnectionError call...");
                string cerror = csve.connectionError(strs[0], "0x80040400", "Something went wrong");
                Console.WriteLine("Writing reponse to file...");
                Logger.logMessage(cerror); 
                Console.WriteLine("Response = " + cerror + " Expecting = DONE");

                Console.WriteLine("Sending getLastError call..");
                cerror = csve.getLastError(strs[0]);
                Console.WriteLine("Writing reponse to file...");
                Logger.logMessage(cerror);
                Console.WriteLine("Response = " + cerror + " Expecting = QuickBooks is not running.");
            }

            Console.WriteLine("Closing Connection...");
            response = csve.closeConnection(strs[0]);
            Console.WriteLine("Writing reponse to file...");
            Logger.logMessage(response);
            Console.WriteLine("Response = " + response + " Expecting = OK");

            return;
        }
    }
}
