using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace qbExportService
{
    public class ContextMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, 
                                          IClientChannel channel, 
                                          InstanceContext instanceContext)
        {
            OperationContext.Current.Extensions.Add(new Context());
            return request.Headers.MessageId;
        }
 
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
           OperationContext.Current.Extensions.Remove(Context.Current);
        } 


    }
}