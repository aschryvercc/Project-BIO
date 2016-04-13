using DbConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CSVExportService
{
    public class CSVExportService : ICSVExportService
    {
        #region Globals
        EventLog eventLog; //Event Logger
        #endregion

        #region utility
        /*
         * Method:  initEventLog()
         * 
         * Parameters: Void 
         * 
         * Returns: Void
         * 
         * Description: Initializes the application for logging events.
         */
        private void initEventLog()
        {
            string source = "qbExportService";

            try
            {
                /*
                 * Create an event log source if one does not exist...
                 */
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, "Application");
                }
                else
                {
                    eventLog = new EventLog();
                    eventLog.Source = source;
                }
            }
            catch (Exception ex)
            {
                /*
                 * Do something with this exception...
                 */
            }

            return;
        }

        /*
         * Method:  logEvent()
         * 
         * Parameters: string logText
         * 
         * Returns: Void
         * 
         * Description: Log the event in the Windows Events Logs.
         */
        private void logEvent(string logText)
        {
            try
            {
                eventLog.WriteEntry(logText);
            }
            catch (Exception ex)
            {
                /*
                 * Do something with this exception...
                 */
            }

            return;
        }
        #endregion

        #region Methods
        public string[] authenticate(string userName, string password)
        {
           string[] resultValue = new string[2];
           string eventText = "";

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : authenticate()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string userName = " + userName + "\r\n";
            eventText += "string password = " + password + "\r\n"; //Also DON'T do this...
            eventText += "\r\n"; 

            resultValue[0] = Guid.NewGuid().ToString();

            //TODO: Add real world authentication for validating username and password
            //That means DON'T do this...
            if (userName.Equals("username") &&
                password.Trim().Equals("thisisbad"))
            {
                /*
                 * Empty string for the second string value
                 */
                resultValue[1] = "";
            }
            else
            {
                resultValue[1] = "nvu";
            }

            eventText += "Return Value       :\r\n";
            eventText += "string resultValue[0] = " + resultValue[0] + "\r\n";
            eventText += "string resultValue[1] = " + resultValue[1] + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

            return resultValue;
        }
        
        public string CSVExport(string token)
        {
            string returnValue = "";
            List<DataRow> ls = new List<DataRow>();

            //Extract table invoice data here

            //DbConnectorInfo sourceInfo = new DbConnectorInfo();
            //DbConnection sourceConnection = new DbConnection(sourceInfo);

            //Dictionary<string, string> leftPair = new Dictionary<string, string>();
            //Dictionary<string, string> rightPair = new Dictionary<string, string>();
            //List<string> columns = new List<string>();
            //List<string> conditions = new List<string>();

            DataTable sourceData = new DataTable();//sourceConnection.PullData(true, leftPair, rightPair, columns, conditions);
            sourceData.Columns.AddRange(new DataColumn[] {
                new DataColumn("ID", typeof(Guid)),
                new DataColumn("Date", typeof(DateTime)),
                new DataColumn("StringValue", typeof(string)),
                new DataColumn("NumberValue", typeof(int)),
                new DataColumn("BooleanValue", typeof(bool))
            });
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String1", 100, true);
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String2", 200, false);
            sourceData.Rows.Add(Guid.NewGuid(), DateTime.Now, "String3", 300, true);

            foreach (DataRow row in sourceData.Rows)
            {
                ls.Add(row);
            }

            CsvExport<DataRow> csve= new CsvExport<DataRow>(ls);
            string eventText = "";

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : authenticate()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string token = " + token + "\r\n";
            eventText += "\r\n";

            try
            {
                returnValue = csve.Export();
            }
            catch(Exception ex)
            {
                logEvent(ex.ToString());
            }

            eventText += "Return Value       :\r\n";
            eventText += "string returnValue = " + returnValue + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

            return returnValue;
        }
        #endregion
    }
}
