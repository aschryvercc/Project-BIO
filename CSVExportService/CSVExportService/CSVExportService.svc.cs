﻿using DbConnector;
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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CSVExportService : ICSVExportService
    {
        #region Globals
        EventLog eventLog; //Event Logger
        string session_id; //Current Session ID
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
            string source = "CSVExportService";

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
         * Returns: StringBuilder sb.ToString()
         * Description: Converts a DataTable to a Csv friendly formatted string and returns it.
         */
        private static string ConvertToCsv(DataTable table)
        {
            StringBuilder sb = new StringBuilder(); //String builder for building the csv string

            /*
             * For each colummn in the data table add the column name.
             */
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                sb.Append(table.Columns[i].ColumnName); //Add the column name.

                /*
                 * If there are more columns, simply add a comma.
                 */
                if (i != table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
                /*
                 * Else end a line...
                 */
                else
                {
                    sb.AppendLine();
                }
            }
            /*
             * For each row in the data table...
             */
            foreach (DataRow row in table.Rows)
            {
                /*
                 * For each colummn in the data table add the column value.
                 */
                for (int i = 0; i < table.Columns.Count; ++i)
                {
                    if (row[i].GetType() == typeof(DateTime))
                    {
                        if (((DateTime)row[i]).TimeOfDay.TotalSeconds == 0)
                        {
                            sb.Append(((DateTime)row[i]).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            sb.Append(((DateTime)row[i]).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                    else if(row[i].ToString().Contains(",") ||
                        row[i].ToString().Contains("\""))
                    {
                        sb.Append('"' + row[i].ToString().Replace("\"", "\"\"") + '"');
                    }
                    else
                    {
                        sb.Append(row[i].ToString()); //Add the column value.
                    }

                    /*
                     * If there are more columns, simply add a comma.
                     */
                    if (i != table.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                    /*
                     * Else end the line...
                     */
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
        /*
         * Method:  authenticate()
         * 
         * Parameters: string userName, 
         *             string password
         * 
         * Returns: string[] resultValue
         *  Possible values:
         *      string[0] = ticket
         *      string[1] =
         *          empty string = authentication is valid
         *          "nvu" = not valid user
         *          any other string value = an error has occured
         * 
         * Description: Verifies a username and password for the web client that is attempting to connect.
         */
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
                resultValue[1] = ""; //Valid user
            }
            else
            {
                resultValue[1] = "nvu"; //Not valid user
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

        /*
         * Method:  CSVExport()
         * 
         * Parameters: string token
         * 
         * Returns: StringBuilder sb.ToString()
         *  Possible values:
         *      Empty String = No data or error
         *      CSV formatted string = Valid Export
         *      Any other string value = an error has occured
         * 
         * Description: Returns a csv formatted string from the source database.
         */
        public string CSVExport(string token)
        {
            string returnValue = ""; //Return value

            StringBuilder sb = new StringBuilder(); // Result of the query.

            /*
             * Extract table invoice data.
             * 
             * Set up database connection.
             */
            DbConnectorInfo sourceInfo = new DbConnectorInfo("localhost\\SQLEXPRESS", "NORTHWND", "sa", "Conestoga1");
            DbConnection sourceConnection = new DbConnection(sourceInfo, "MS");

            /*
             * Set up query for database.
             */
            ; ;

            sb.Append("Connecting to source...").AppendLine();
            sb.AppendFormat("DbConnectorInfo:").AppendLine();
            sb.AppendFormat("server: {0}", sourceInfo.server).AppendLine();
            sb.AppendFormat("database: {0}", sourceInfo.database).AppendLine();
            sb.AppendFormat("userid: {0}", sourceInfo.userid).AppendLine();

            /*
             * Open the source and destination databases.
             */
            if (!sourceConnection.ConnectionOpen)
            {
                sourceConnection.OpenDBConnection();
            }

            /*
             * Pull data from BioLinks.
             */
            DataTable sourceData = sourceConnection.pullData("Orders", null, null);

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
                returnValue = ConvertToCsv(sourceData); //Get the csv formatted string from the datatable
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

            /*
             * Close the source and destination databases.
             */
            sourceConnection.CloseDBConnection();

            return returnValue;
        }
        #endregion
    }
}