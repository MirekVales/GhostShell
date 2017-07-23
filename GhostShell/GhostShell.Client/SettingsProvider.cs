using System;
using Microsoft.Win32;

namespace GhostShell.Client
{
    public class SettingsProvider
    {
        public string Endpoint { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public SettingsProvider()
        {
            Endpoint = @"net.tcp://localhost:61234/GhostShellBroker";
            Timeout = TimeSpan.FromMilliseconds(1000);

            try
            {
                TryLoadSettingsFromRegistry();
            }
            catch
            { }
        }

        private void TryLoadSettingsFromRegistry()
        {
            var key = Registry.ClassesRoot?.OpenSubKey("CLSID")?.OpenSubKey(typeof(GhostShellClient).GUID.ToString());
            if (key == null)
                return;

            Endpoint = (string)key.GetValue("BrokerEndpoint", Endpoint);
            var timeout = (string)key.GetValue("BrokerTimeout", Timeout.ToString());
            Timeout = TimeSpan.Parse(timeout);
        }
    }
}
