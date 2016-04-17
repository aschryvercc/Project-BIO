﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceTestHarness.qbExportService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="qbExportService.IqbExportService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IqbExportService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IqbExportService/authenticate", ReplyAction="http://tempuri.org/IqbExportService/authenticateResponse")]
        string[] authenticate(string userName, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IqbExportService/authenticate", ReplyAction="http://tempuri.org/IqbExportService/authenticateResponse")]
        System.Threading.Tasks.Task<string[]> authenticateAsync(string userName, string password);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/clientVersion", ReplyAction="http://tempuri.org/IqbExportService/clientVersionResponse")]
        string clientVersion(string version);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/clientVersion", ReplyAction="http://tempuri.org/IqbExportService/clientVersionResponse")]
        System.Threading.Tasks.Task<string> clientVersionAsync(string version);
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, IsInitiating=false, Action="http://tempuri.org/IqbExportService/closeConnection", ReplyAction="http://tempuri.org/IqbExportService/closeConnectionResponse")]
        string closeConnection(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, IsInitiating=false, Action="http://tempuri.org/IqbExportService/closeConnection", ReplyAction="http://tempuri.org/IqbExportService/closeConnectionResponse")]
        System.Threading.Tasks.Task<string> closeConnectionAsync(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/connectionError", ReplyAction="http://tempuri.org/IqbExportService/connectionErrorResponse")]
        string connectionError(string ticket, string result, string msg);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/connectionError", ReplyAction="http://tempuri.org/IqbExportService/connectionErrorResponse")]
        System.Threading.Tasks.Task<string> connectionErrorAsync(string ticket, string result, string msg);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/getLastError", ReplyAction="http://tempuri.org/IqbExportService/getLastErrorResponse")]
        string getLastError(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/getLastError", ReplyAction="http://tempuri.org/IqbExportService/getLastErrorResponse")]
        System.Threading.Tasks.Task<string> getLastErrorAsync(string ticket);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/receiveResponseXML", ReplyAction="http://tempuri.org/IqbExportService/receiveResponseXMLResponse")]
        int receiveResponseXML(string ticket, string response, string result, string msg);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/receiveResponseXML", ReplyAction="http://tempuri.org/IqbExportService/receiveResponseXMLResponse")]
        System.Threading.Tasks.Task<int> receiveResponseXMLAsync(string ticket, string response, string result, string msg);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/sendRequestXML", ReplyAction="http://tempuri.org/IqbExportService/sendRequestXMLResponse")]
        string sendRequestXML(string ticket, string response, string companyFileName, string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion);
        
        [System.ServiceModel.OperationContractAttribute(IsInitiating=false, Action="http://tempuri.org/IqbExportService/sendRequestXML", ReplyAction="http://tempuri.org/IqbExportService/sendRequestXMLResponse")]
        System.Threading.Tasks.Task<string> sendRequestXMLAsync(string ticket, string response, string companyFileName, string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IqbExportServiceChannel : ServiceTestHarness.qbExportService.IqbExportService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IqbExportServiceClient : System.ServiceModel.ClientBase<ServiceTestHarness.qbExportService.IqbExportService>, ServiceTestHarness.qbExportService.IqbExportService {
        
        public IqbExportServiceClient() {
        }
        
        public IqbExportServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IqbExportServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IqbExportServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IqbExportServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] authenticate(string userName, string password) {
            return base.Channel.authenticate(userName, password);
        }
        
        public System.Threading.Tasks.Task<string[]> authenticateAsync(string userName, string password) {
            return base.Channel.authenticateAsync(userName, password);
        }
        
        public string clientVersion(string version) {
            return base.Channel.clientVersion(version);
        }
        
        public System.Threading.Tasks.Task<string> clientVersionAsync(string version) {
            return base.Channel.clientVersionAsync(version);
        }
        
        public string closeConnection(string ticket) {
            return base.Channel.closeConnection(ticket);
        }
        
        public System.Threading.Tasks.Task<string> closeConnectionAsync(string ticket) {
            return base.Channel.closeConnectionAsync(ticket);
        }
        
        public string connectionError(string ticket, string result, string msg) {
            return base.Channel.connectionError(ticket, result, msg);
        }
        
        public System.Threading.Tasks.Task<string> connectionErrorAsync(string ticket, string result, string msg) {
            return base.Channel.connectionErrorAsync(ticket, result, msg);
        }
        
        public string getLastError(string ticket) {
            return base.Channel.getLastError(ticket);
        }
        
        public System.Threading.Tasks.Task<string> getLastErrorAsync(string ticket) {
            return base.Channel.getLastErrorAsync(ticket);
        }
        
        public int receiveResponseXML(string ticket, string response, string result, string msg) {
            return base.Channel.receiveResponseXML(ticket, response, result, msg);
        }
        
        public System.Threading.Tasks.Task<int> receiveResponseXMLAsync(string ticket, string response, string result, string msg) {
            return base.Channel.receiveResponseXMLAsync(ticket, response, result, msg);
        }
        
        public string sendRequestXML(string ticket, string response, string companyFileName, string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion) {
            return base.Channel.sendRequestXML(ticket, response, companyFileName, qbXMLCountry, qbXMLMajorVersion, qbXMLMinorVersion);
        }
        
        public System.Threading.Tasks.Task<string> sendRequestXMLAsync(string ticket, string response, string companyFileName, string qbXMLCountry, int qbXMLMajorVersion, int qbXMLMinorVersion) {
            return base.Channel.sendRequestXMLAsync(ticket, response, companyFileName, qbXMLCountry, qbXMLMajorVersion, qbXMLMinorVersion);
        }
    }
}