using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace qbExportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IqbExportService" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IqbExportService
    {
        [OperationContract(IsInitiating = true)]
        string[] authenticate(string userName, string password);

        [OperationContract(IsInitiating = false)]
        string clientVersion(string version);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        string closeConnection(string ticket);

        [OperationContract(IsInitiating = false)]
        string connectionError(string ticket, string result, string msg);
        
        [OperationContract(IsInitiating = false)]
        string getLastError(string ticket);

        [OperationContract(IsInitiating = false)]
        int receiveResponseXML(string ticket, string response, string result, string msg);

        [OperationContract(IsInitiating = false)]
        string sendRequestXML(string ticket, string response, string companyFileName,
                              string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion);
    }
}
