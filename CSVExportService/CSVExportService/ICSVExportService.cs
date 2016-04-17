using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CSVExportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICSVExportService
    {
        [OperationContract(IsInitiating = true)]
        string[] authenticate(string username, string password);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        string CSVExport(string token);
    }
}
