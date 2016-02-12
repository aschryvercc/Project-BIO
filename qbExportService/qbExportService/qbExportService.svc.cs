﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.Diagnostics; //EventLog

namespace qbExportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "qbExportService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select qbExportService.svc or qbExportService.svc.cs at the Solution Explorer and start debugging.
    public class qbExportService : IqbExportService
    {
        #region Globals
        EventLog eventLog; //Event Logger
        #endregion

        #region Constructor
        public qbExportService()
        {
            /*
             * Initialize event logging
             */
            initEventLog();
        }

        #endregion

        #region Utility
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                /*
                 * Do something with this exception...
                 */
            }

            return;
        }

        private void addEventLogText(string message)
        {

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
         *          empty string = use current company file
         *          "none" = no further request/no further action required
         *          "nvu" = not valid user
         *          any other string value = use this company file
         * 
         * Description: Verifies a username and password for the web connector that is attempting to connect.
         */
        public string[] authenticate(string userName, string password)
        {
            string[] resultValue = new string[2];

            return resultValue;
        }

        /*
         * Method:  clientVersion()
         * 
         * Parameters: string version
         * 
         * Returns: string resultValue
         *  NULL or <emptyString> = QBWC will let the web service update
         *  "E:<any text>" = popup ERROR dialog with <any text>
         *                   abort update and force download of new QBWC.
         *  "W:<any text>" = popup WARNING dialog with <any text>
         *                   choice to user, continue update or not.
         * 
         * Description: QBWC version control.
         */
        public string clientVersion(string version)
        {
            string resultValue = null;
            string eventText = "";

            double clientVersion = Convert.ToDouble(this.parseVersion(version));

            /*
             * Update these for version control.
             */
            double recommendedVersion = 1.0;
            double minimumVersion = 1.0;

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : clientVersion()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string version = " + version + "\r\n";
            eventText += "\r\n";

            eventText += "QBWC Version              : " + version + "\r\n";
            eventText += "Recommended Version       : " + recommendedVersion + "\r\n";
            eventText += "Minimum Version           : " + minimumVersion + "\r\n";
            eventText += "Client Version            : " + version.ToString() + "\r\n";

            /*
             * Log Errors and Warnings
             */
            if (clientVersion < recommendedVersion)
            {
                resultValue = "W:It is reccomended to upgrade your QBWebConnector";
            }
            else if(clientVersion < minimumVersion)
            {
                resultValue = "E:You need to upgrade your QBWebConnector";
            }
            eventText += "\r\n";

            eventText += "Return Value       :\r\n";
            eventText += "string resultValue = " + resultValue + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

            return resultValue;
        }


        /*
         * Method:  closeConnection()
         * 
         * Parameters: string ticket
         * 
         * Returns: resultValue
         * 
         * Description: Called at the end of a succesful session.
         */
        public string closeConnection(string ticket)
        {
            string resultValue = null;

            return resultValue;
        }

        /*
         * Method:  initEventLog()
         * 
         * Parameters: Void 
         * 
         * Returns: Void
         * 
         * Description: Initializes the application for logging events.
         */
        public string connectionError(string ticket, string result, string msg)
        {
            string resultValue = null;
            string eventText = "";

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : closeConnection()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
            eventText += "string result = " + result + "\r\n";
            eventText += "string msg = " + msg + "\r\n";
            eventText += "\r\n";

            /*
             * TODO: Determine Return Value.
             */
            resultValue = "OK";

            eventText += "Return Value       :\r\n";
            eventText += "string resultValue = " + resultValue + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

            return resultValue;
        }

        /*
         * Method:  getLastError()
         * 
         * Parameters:  string ticket
         * 
         * Returns: string resultValue
         * 
         * Description: Returns a string about the last error that has occured.
         */
        public string getLastError(string ticket)
        {
            string resultValue = null;

            return resultValue;
        }

        /*
         * Method:  receiveResponseXML()
         * 
         * Parameters: string ticket, 
         *             string response, 
         *             string result, 
         *             string msg
         * 
         * Returns: int resultValue
         * 
         * Description: ...
         */
        public int receiveResponseXML(string ticket, string response, string result, string msg)
        {
            int resultValue = 0;

            return resultValue;
        }

        /*
         * Method:  sendRequestXML()
         * 
         * Parameters: string ticket, 
         *             string response, 
         *             string companyFileName, 
         *             string qbXMLCountry, 
         *             int qbXMLMajorVersion, 
         *             int qbXMLMinorVersion
         * 
         * Returns: string resultValue
         * 
         * Description: ...
         */
        public string sendRequestXML(string ticket, string response, string companyFileName,
                                     string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion)
        {
            string resultValue = null;

            return resultValue;
        }
        #endregion
    }
}
