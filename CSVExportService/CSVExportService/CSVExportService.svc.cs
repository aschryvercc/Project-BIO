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
        string session_id;
        #endregion

        #region Constructor
        public CSVExportService()
        {
            /*
             * Initialize event logging
             */
            initEventLog();
            /*
             * Initialize session variables
             */
            session_id = "";
        }
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
        /*
         * Method:  ConvertToCsv()
         * Parameters:  table - DataTable to be converted to Csv friendly format.
         * Description: Converts a DataTable to a Csv friendly formatted string and returns it.
         */
        private static string ConvertToCsv(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                sb.Append(table.Columns[i].ColumnName);
                if (i != table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; ++i)
                {
                    sb.Append(row[i].ToString());
                    if (i != table.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
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

            session_id = Guid.NewGuid().ToString();
            resultValue[0] = session_id;

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
            List<string> ls = new List<string>();

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
                returnValue = ConvertToCsv(sourceData);
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