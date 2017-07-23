using System;
using System.ServiceModel;
using System.Collections.Generic;
using GhostShell.Controls;
using GhostShell.Behaviour;

namespace GhostShell.Client
{
    public static class WcfClient
    {
        private static readonly NetTcpBinding myBinding;
        private static readonly EndpointAddress myEndpoint;
        private static readonly ChannelFactory<IBroker> myChannelFactory;

        static WcfClient()
        {
            var settings = new SettingsProvider();
            myBinding = new NetTcpBinding()
            {
                OpenTimeout = settings.Timeout,
                CloseTimeout = settings.Timeout,
                SendTimeout = settings.Timeout,
                ReceiveTimeout = settings.Timeout,
                MaxReceivedMessageSize = 2097152,
                MaxBufferSize = 2097152,
                MaxBufferPoolSize = 2097152,
            };
            myEndpoint = new EndpointAddress(settings.Endpoint);
            myChannelFactory = new ChannelFactory<IBroker>(myBinding, myEndpoint);
        }

        public static Item[] GetMenuItems(IEnumerable<string> selectedPaths)
            => Invoke(client => client.GetMenuItems(new SelectedItems(selectedPaths)));

        public static bool Execute(Guid id)
            => Invoke(client => client.ExecuteOperation(id));

        private static T Invoke<T>(Func<IBroker, T> action)
        {
            IBroker client = null;

            try
            {
                client = myChannelFactory.CreateChannel();
                var result = action(client);
                ((ICommunicationObject)client).Close();
                return result;
            }
            catch
            {
                if (client != null)
                    ((ICommunicationObject)client).Abort();
                return default(T);
            }
        }
    }
}
