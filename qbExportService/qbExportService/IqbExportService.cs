using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace qbExportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IqbExportService" in both code and config file together.
    [ServiceContract]
    public interface IqbExportService
    {
        [OperationContract]
        string[] authenticate(string userName, string password);

        [OperationContract]
        string clientVersion(string version);

        [OperationContract]
        string closeConnection(string ticket);

        [OperationContract]
        string connectionError(string ticket, string result, string msg);

        [OperationContract]
        string getLastError(string ticket);

        [OperationContract]
        int receiveResponseXML(string ticket, string response, string result, string msg);

        [OperationContract]
        string sendRequestXML(string ticket, string response, string companyFileName,
                              string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion);
    }
}
