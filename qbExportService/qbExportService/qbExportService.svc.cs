using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.Diagnostics; //EventLog
using System.Collections; //ArrayList
using System.Text.RegularExpressions; //Regex

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

        /*
         * Method:  parseVersion()
         * 
         * Parameters: string versionNumber
         * 
         * Returns: resultValue.
         * 
         * Description: Returns the major and minor components from a standard version number, otherwise an empty string.
         */
        private string parseVersion(string versionNumber)
        {
            string resultValue = "";
            string majorNumber = "";
            string minorNumebr = "";
            
            Regex regexVersion = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)(\.\w+){0,2}$", RegexOptions.Compiled);

            Match matchVersion = regexVersion.Match(versionNumber);

            /*
             * Get the version numbers major and minor components, 
             * if the version matches.
             */
            if (matchVersion.Success)
            {
                majorNumber = matchVersion.Result("${major}");
                minorNumebr = matchVersion.Result("${minor}");

                resultValue = majorNumber + "." + minorNumebr;
            }

            return resultValue;
        }

        private ArrayList buildRequest()
        {
            ArrayList req = new ArrayList();

            /*
             * TODO: Figure out how to build a proper request(s).
             */

            return req;
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
                 * Empty string for the second string value asking QBWebConnector 
                 * to connect to the company file that is currently opened in QB.
                 */
                resultValue[1] = "c:\\Program Files\\Intuit\\QuickBooks\\sample_product-based business.qbw"; //This is from the sample file...
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
        public string clientVersion(string versionNumber)
        {
            string resultValue = null;
            string eventText = "";

            /*
             * Get the major and minor parts from the version number.
             */
            double clientVersion = Convert.ToDouble(this.parseVersion(versionNumber));

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
            eventText += "string version = " + versionNumber + "\r\n";
            eventText += "\r\n";

            eventText += "QBWC Version              : " + versionNumber + "\r\n";
            eventText += "Recommended Version       : " + recommendedVersion + "\r\n";
            eventText += "Minimum Version           : " + minimumVersion + "\r\n";
            eventText += "Client Version            : " + clientVersion.ToString() + "\r\n";

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
            string eventText = "";

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : closeConnection()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
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
             * Constant string values for QB errors.
             * Add values as you need them.
             */
            const string QB_ERROR_WHEN_PARSING = "0x80040400"; // 0x80040400 - QuickBooks found an error when parsing the provided XML text stream 
            const string QB_COULDNT_ACCESS_QB = "0x80040401";  // 0x80040401 - Could not access QuickBooks. 
            const string QB_UNEXPECTED_ERROR = "0x80040402";   // 0x80040402 - Unexpected error. Check the qbsdklog.txt file for possible, additional information. 

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : connectionError()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
            eventText += "string result = " + result + "\r\n";
            eventText += "string msg = " + msg + "\r\n";
            eventText += "\r\n";

            if (Context.Current.Ce_counter == null)
            {
                Context.Current.Ce_counter = 0;
            }

            /*
             * Determine error code.
             */
            if (result.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {

            }
            else if (result.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {

            }
            else if (result.Trim().Equals(QB_UNEXPECTED_ERROR))
            {

            }
            else
            {
                if (Context.Current.Ce_counter == 0)
                {

                }
                else
                {

                }
            }

            eventText += "\r\n";
            eventText += "Return Value       :\r\n";
            eventText += "string resultValue = " + resultValue + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

            /*
             * Increase connection error count.
             */
            Context.Current.Ce_counter += 1;

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
            string eventText = "";

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : getLastError()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
            eventText += "\r\n";

            /*
             * TODO: Determine last error code... maybe add a global.
             * For now just return a fake message.
             */
            resultValue = "QuickBooks is not running.";


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
            int requestCount = 0;
            int totalRequests = 0;
            int percentage = 0;
            string eventText = "";
            ArrayList request = null;

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : recieveResponseXML()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
            eventText += "string response = " + response + "\r\n";
            eventText += "string result = " + result + "\r\n";
            eventText += "string msg = " + msg + "\r\n";
            eventText += "\r\n";

            /*
             * When a error occurs, return a error code in the form of a -ve int.
             */ 
            if (!result.ToString().Equals(""))
            {
                resultValue = -101;
            }
            else
            {
                eventText += "Lenght of response revieved = " + response.Length + "\r\n";

                request = buildRequest();

                totalRequests = request.Count;
                requestCount = Convert.ToInt32(Context.Current.Counter);

                percentage = (requestCount * 100) / totalRequests;
                
                if (percentage >= 100)
                {
                    requestCount = 0;
                    Context.Current.Counter = 0;
                }

                resultValue = percentage;
            }

            eventText += "\r\n";
            eventText += "Return Value       :\r\n";
            eventText += "string resultValue = " + resultValue.ToString() + "\r\n";
            eventText += "\r\n";

            /*
             * Log the event.
             */
            logEvent(eventText);

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
