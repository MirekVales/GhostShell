using System;
using System.ServiceModel;
using System.Drawing;
using System.Windows.Forms;
using GhostShell.Behaviour;

namespace GhostShell.Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            var clientPath = @"A PATH TO CLIENT DLL (GhostShell.Client.dll)";

            using (var clientRegistration = new Registration(clientPath))
            {
                var registration = Association.Create
                    .AssociatedToFile("*.*")
                    .AssociatedToDirectory()
                    .WithItem(new Controls.Separator(),
                    new Controls.MenuItem("Application", SystemIcons.Application.ToBitmap(),
                        new Controls.MenuItem("Asterisk", SystemIcons.Asterisk.ToBitmap(), () => MessageBox.Show("Asterisk has been pressed")),
                        new Controls.MenuItem("Warning", SystemIcons.Warning.ToBitmap(), () => MessageBox.Show("Warning has been pressed")),
                        new Controls.MenuItem("Hand", SystemIcons.Hand.ToBitmap())), new Controls.Separator());

                var broker = new Broker();
                broker.RegisterExpression(registration);

                var url = @"net.tcp://localhost:61234/GhostShellBroker";
                using (var host = new ServiceHost(broker, new Uri(url)))
                {
                    host.AddServiceEndpoint(typeof(IBroker), new NetTcpBinding(), url);
                    host.Open();

                    Console.ReadLine();
                }

            }
        }
    }
}
