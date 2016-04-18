using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace qbExportService
{
    /*
     * Set the service contract to require sessions.
     */
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IqbExportService
    {
        /*
         * Operation required to start the session.
         */
        [OperationContract(IsInitiating = true)]
        string[] authenticate(string userName, string password);
        /* 
         * Operations available during the session.
         */
        [OperationContract(IsInitiating = false)]
        string clientVersion(string version);
        
        [OperationContract(IsInitiating = false)]
        string connectionError(string ticket, string result, string msg);
        
        [OperationContract(IsInitiating = false)]
        string getLastError(string ticket);

        [OperationContract(IsInitiating = false)]
        int receiveResponseXML(string ticket, string response, string result, string msg);

        [OperationContract(IsInitiating = false)]
        string sendRequestXML(string ticket, string response, string companyFileName,
                              string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion);
        /*
         * Operation required to terminate the session.
         */
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        string closeConnection(string ticket);
    }
}
