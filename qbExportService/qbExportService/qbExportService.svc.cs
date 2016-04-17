using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.Diagnostics; //EventLog
using System.Collections; //ArrayList
using System.Text.RegularExpressions;
using System.Xml; //Regex

namespace qbExportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "qbExportService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select qbExportService.svc or qbExportService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class qbExportService : IqbExportService
    {
        #region Globals
        EventLog eventLog; //Event Logger
        int ce_counter;
        int counter;
        string session_id;
        #endregion

        #region Constructor
        public qbExportService()
        {
            /*
             * Initialize event logging
             */
            initEventLog();
            /*
             * Initialize session variables
             */
            ce_counter = 0;
            counter = 0;
            session_id = "";
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
            string strRequestXML = "";
            XmlDocument inputXMLDocument = null;
            ArrayList req = new ArrayList();

            /* 
             * InvoiceQuery
             */
            inputXMLDocument = new XmlDocument();
            inputXMLDocument.AppendChild(inputXMLDocument.CreateXmlDeclaration("1.0", null, null));
            inputXMLDocument.AppendChild(inputXMLDocument.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            XmlElement qbXML = inputXMLDocument.CreateElement("QBXML");
            inputXMLDocument.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDocument.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement invoiceQueryRq = inputXMLDocument.CreateElement("InvoiceQueryRq");
            qbXMLMsgsRq.AppendChild(invoiceQueryRq);
            invoiceQueryRq.SetAttribute("requestID", "2");
            XmlElement maxReturned = inputXMLDocument.CreateElement("MaxReturned");
            invoiceQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDocument.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDocument = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            maxReturned = null;

            /*
             * InvoiceAdd
             */
            inputXMLDocument = new XmlDocument();
            inputXMLDocument.AppendChild(inputXMLDocument.CreateXmlDeclaration("1.0", null, null));
            inputXMLDocument.AppendChild(inputXMLDocument.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            qbXML = inputXMLDocument.CreateElement("QBXML");
            inputXMLDocument.AppendChild(qbXML);

            qbXMLMsgsRq = inputXMLDocument.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");

            XmlElement invoiceAddRq = inputXMLDocument.CreateElement("InvoiceAddRq");
            qbXMLMsgsRq.AppendChild(invoiceAddRq);
            invoiceAddRq.SetAttribute("requestID", "2");

            XmlElement invoiceAdd = inputXMLDocument.CreateElement("InvoiceAdd");
            invoiceAddRq.AppendChild(invoiceAdd);

            XmlElement customerRef = inputXMLDocument.CreateElement("CustomerRef");
            invoiceAdd.AppendChild(customerRef);

            XmlElement listID = inputXMLDocument.CreateElement("ListID");
            customerRef.AppendChild(listID).InnerText = "90001-1263558758";
            XmlElement fullName = inputXMLDocument.CreateElement("FullName");
            customerRef.AppendChild(fullName).InnerText = "Testy McTestFace";

            XmlElement txnDate = inputXMLDocument.CreateElement("TxnDate");
            invoiceAdd.AppendChild(txnDate).InnerText = "2016-03-15";

            XmlElement refNumber = inputXMLDocument.CreateElement("RefNumeber");
            invoiceAdd.AppendChild(refNumber).InnerText = "21011";

            XmlElement billAddress = inputXMLDocument.CreateElement("BillAddress");
            invoiceAdd.AppendChild(billAddress);
            XmlElement addr1 = inputXMLDocument.CreateElement("Addr1");
            billAddress.AppendChild(addr1).InnerText = "123 Test Road";
            XmlElement addr2 = inputXMLDocument.CreateElement("Addr2");
            billAddress.AppendChild(addr2).InnerText = "456 Face Road";
            XmlElement addr3 = inputXMLDocument.CreateElement("Addr3");
            billAddress.AppendChild(addr3);
            XmlElement city = inputXMLDocument.CreateElement("City");
            billAddress.AppendChild(city).InnerText = "Testing";
            XmlElement state = inputXMLDocument.CreateElement("State");
            billAddress.AppendChild(state).InnerText = "State";
            XmlElement postalCode = inputXMLDocument.CreateElement("PostalCode");
            billAddress.AppendChild(postalCode).InnerText = "06279";
            XmlElement country = inputXMLDocument.CreateElement("Country");
            billAddress.AppendChild(country).InnerText = "Canada";

            XmlElement shipAddress = inputXMLDocument.CreateElement("ShipAddress");
            invoiceAdd.AppendChild(shipAddress);
            addr1 = inputXMLDocument.CreateElement("Addr1");
            shipAddress.AppendChild(addr1).InnerText = "123 Test2 Road";
            addr2 = inputXMLDocument.CreateElement("Addr2");
            shipAddress.AppendChild(addr2).InnerText = "456 Face2 Road";
            addr3 = inputXMLDocument.CreateElement("Addr3");
            shipAddress.AppendChild(addr3);
            city = inputXMLDocument.CreateElement("City");
            shipAddress.AppendChild(city).InnerText = "Testing2";
            state = inputXMLDocument.CreateElement("State");
            shipAddress.AppendChild(state).InnerText = "State2";
            postalCode = inputXMLDocument.CreateElement("PostalCode");
            shipAddress.AppendChild(postalCode).InnerText = "01234";
            country = inputXMLDocument.CreateElement("Country");
            shipAddress.AppendChild(country).InnerText = "Canada";

            XmlElement termsRef = inputXMLDocument.CreateElement("TermsRef");
            invoiceAdd.AppendChild(termsRef);
            fullName = inputXMLDocument.CreateElement("FullName");
            termsRef.AppendChild(fullName).InnerText = "Mr. Krabs";

            XmlElement salesRepRef = inputXMLDocument.CreateElement("SalesRepRef");
            invoiceAdd.AppendChild(salesRepRef);
            fullName = inputXMLDocument.CreateElement("FullName");
            termsRef.AppendChild(fullName).InnerText = "Plankton";

            XmlElement memo = inputXMLDocument.CreateElement("Memo");
            invoiceAdd.AppendChild(memo).InnerText = "Memo McMemoFace";

            XmlElement invoiceLineAdd = inputXMLDocument.CreateElement("InvoiceLineAdd");
            invoiceAdd.AppendChild(invoiceLineAdd);

            XmlElement itemRef = inputXMLDocument.CreateElement("ItemRef");
            invoiceLineAdd.AppendChild(itemRef);
            fullName = inputXMLDocument.CreateElement("FullName");
            itemRef.AppendChild(fullName).InnerText = "Tester";
            XmlElement desc = inputXMLDocument.CreateElement("Desc");
            itemRef.AppendChild(desc).InnerText = "Tester";
            XmlElement quantity = inputXMLDocument.CreateElement("Quantity");
            itemRef.AppendChild(quantity).InnerText = "4.00000";
            XmlElement rate = inputXMLDocument.CreateElement("Rate");
            itemRef.AppendChild(rate).InnerText = "25.00000";

            strRequestXML = inputXMLDocument.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDocument = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            invoiceAddRq = null;
            invoiceAdd = null;
            customerRef = null;
            listID = null;
            fullName = null;
            txnDate = null;
            refNumber = null;
            billAddress = null;
            addr1 = null;
            addr2 = null;
            addr3 = null;
            city = null;
            state = null;
            postalCode = null;
            country = null;
            shipAddress = null;
            termsRef = null;
            salesRepRef = null;
            memo = null;
            invoiceLineAdd = null;
            itemRef = null;
            desc = null;
            quantity = null;
            rate = null;

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

            session_id = Guid.NewGuid().ToString();
            resultValue[0] = session_id;

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
        public string connectionError(string ticket, string hresult, string msg)
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
            eventText += "string result = " + hresult + "\r\n";
            eventText += "string msg = " + msg + "\r\n";
            eventText += "\r\n";

            if (ce_counter == null)
            {
                ce_counter = 0;
            }

            /*
             * Determine error code.
             */
            if (hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                resultValue = "DONE";
            }
            else if (hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                resultValue = "DONE";
            }
            else if (hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                resultValue = "DONE";
            }
            else
            {
                if (ce_counter == 0)
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
            ce_counter += 1;

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
         * Description: Returns the data request response from QuickBooks or QuickBooks POS.
         * A positive integer less than 100 represents the percentage of work completed. A value of 1
         * means one percent complete, a value of 100 means 100 percent complete--there is no more
         * work. A negative value means an error has occurred and the Web Connector responds to
         * this with a getLastError call. The negative value could be used as a custom error code
         */
        public int receiveResponseXML(string ticket, string response, string hresult, string msg)
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
            eventText += "string result = " + hresult + "\r\n";
            eventText += "string msg = " + msg + "\r\n";
            eventText += "\r\n";

            /*
             * When a error occurs, return a error code in the form of a -ve int.
             */ 
            if (!hresult.ToString().Equals(""))
            {
                resultValue = -101;
            }
            else
            {
                eventText += "Length of response revieved = " + response.Length + "\r\n";

                request = buildRequest();

                totalRequests = request.Count;
                requestCount = Convert.ToInt32(counter);

                //Do something with Quick Books response here..

                /*
                 * Set up the return value:
                 * Greater than zero  = There are more request to send
                 * 100 = Done. no more request to send
                 * Less than zero  = Custom Error codes
                 */
                percentage = (requestCount * 100) / totalRequests;
                
                if (percentage >= 100)
                {
                    requestCount = 0;
                    counter = 0;
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
         * Description: The web connector’s invitation to the web service to send a request. 
         * After receiving the session token (ticket) returned from the web service in response to the
         * authenticate call, the web connector establishes a connection to QuickBooks using QBXML
         * Request Processor. The web connector then calls sendRequestXML, supplying in that call
         * certain information about the QuickBooks or QuickBooks POS connection that the web
         * connector has established.
         * If there is a problem establishing the connection the web connector does not call
         * sendRequestXML, but instead calls connectionError.
         */
        public string sendRequestXML(string ticket, string response, string companyFileName,
                                     string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion)
        {
            string resultValue = null;
            string eventText = "";
            ArrayList request = buildRequest();
            int totalRequests = request.Count;

            if (counter == null)
            {
                counter = 0;
            }

            int requestCount = counter;

            /*
             * Build the event log. 
             */
            eventText += "WebMethod        : sendResponseXML()\r\n\r\n";
            eventText += "Parameters       :\r\n";
            eventText += "string ticket = " + ticket + "\r\n";
            eventText += "string response = " + response + "\r\n";
            eventText += "string companyFileName = " + companyFileName + "\r\n";
            eventText += "string qbXMLCountry = " + qbXMLCountry + "\r\n";
            eventText += "string qbXMLMajorVersion = " + qbXMLMajorVersion + "\r\n";
            eventText += "string qbXMLMinorVersion = " + qbXMLMinorVersion + "\r\n";
            eventText += "\r\n";

            //Send Quick Books a message.
            if (requestCount < totalRequests)
            {
                resultValue = request[requestCount].ToString();
                eventText += "Sending request number = " + (requestCount + 1) + "\r\n";
                counter += 1;
            }
            else
            {
                requestCount = 0;
                counter = 0;
                resultValue = "";
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
        #endregion
    }
}
