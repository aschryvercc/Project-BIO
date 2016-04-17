﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace qbExportService
{
    public class ContextBehaviourAttribute : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
            //no-op
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.MessageInspectors.Add(new ContextMessageInspector());
                }
            }
        }
 
        public void Validate(ServiceDescription serviceDescription,ServiceHostBase serviceHostBase)
        {
            //no-op           
        }

        #endregion

    }
}