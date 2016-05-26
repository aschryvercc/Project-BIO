﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceConsumer.CSVExportService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CSVExportService.ICSVExportService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface ICSVExportService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICSVExportService/authenticate", ReplyAction="http://tempuri.org/ICSVExportService/authenticateResponse")]
        string[] authenticate(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICSVExportService/authenticate", ReplyAction="http://tempuri.org/ICSVExportService/authenticateResponse")]
        System.Threading.Tasks.Task<string[]> authenticateAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, IsInitiating=false, Action="http://tempuri.org/ICSVExportService/CSVExport", ReplyAction="http://tempuri.org/ICSVExportService/CSVExportResponse")]
        string CSVExport(string token);
        
        [System.ServiceModel.OperationContractAttribute(IsTerminating=true, IsInitiating=false, Action="http://tempuri.org/ICSVExportService/CSVExport", ReplyAction="http://tempuri.org/ICSVExportService/CSVExportResponse")]
        System.Threading.Tasks.Task<string> CSVExportAsync(string token);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICSVExportServiceChannel : ServiceConsumer.CSVExportService.ICSVExportService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CSVExportServiceClient : System.ServiceModel.ClientBase<ServiceConsumer.CSVExportService.ICSVExportService>, ServiceConsumer.CSVExportService.ICSVExportService {
        
        public CSVExportServiceClient() {
        }
        
        public CSVExportServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CSVExportServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CSVExportServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CSVExportServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] authenticate(string username, string password) {
            return base.Channel.authenticate(username, password);
        }
        
        public System.Threading.Tasks.Task<string[]> authenticateAsync(string username, string password) {
            return base.Channel.authenticateAsync(username, password);
        }
        
        public string CSVExport(string token) {
            return base.Channel.CSVExport(token);
        }
        
        public System.Threading.Tasks.Task<string> CSVExportAsync(string token) {
            return base.Channel.CSVExportAsync(token);
        }
    }
}