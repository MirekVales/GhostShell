# GhostShell
Divides the extension scope to an infrastructure and implementation part - to a client that is registered as a shell extension and invokes features remotely, and to a server that provides an implementation. 
The client has no implementation, therefore it may be registered once and stay intact. The server is invoked over WCF and may be changed on the fly without need to re-register the extension.  

GhostShell uses [SharpShell](https://github.com/dwmkerr/sharpshell) to register shell extensions. 

## Example of use
![Example of use](https://github.com/MirekVales/GhostShell/blob/master/ExampleOfUseScreenshot.png "Example of use")

```csharp
var clientPath = @"A PATH TO CLIENT DLL (GhostShell.Client.dll)";
using (var clientRegistration = new Registration(clientPath))
{
      var registration = Association.Create
          .AssociatedToFile("*.*")
          .WithItem(new Controls.Separator(),
          new Controls.MenuItem("Application", SystemIcons.Application.ToBitmap(),
             new Controls.MenuItem("Asterisk", SystemIcons.Asterisk.ToBitmap(), () => MessageBox.Show("*")),
             new Controls.MenuItem("Warning", SystemIcons.Warning.ToBitmap(), () => MessageBox.Show("!")),
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
```

It is necessary to execute the registration in administrator mode. 
