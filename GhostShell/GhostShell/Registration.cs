using SharpShell.ServerRegistration;
using SharpShell;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System;

namespace GhostShell
{
    public class Registration : IDisposable
    {
        readonly string path;

        public Registration(string path)
            => InstallDll(this.path = path);

        public void Dispose()
            => UninstallDll(path);

        static void InstallDll(string path)
        {
            var serverTypes = LoadServerTypes(path);
            foreach (var serverType in serverTypes)
                foreach (var type in new[] { RegistrationType.OS32Bit, RegistrationType.OS64Bit })
                {
                    ServerRegistrationManager.InstallServer(serverType, type, true);
                    ServerRegistrationManager.RegisterServer(serverType, type);
                }
        }

        static void UninstallDll(string path)
        {
            var serverTypes = LoadServerTypes(path);
            foreach (var serverType in serverTypes)
                foreach (var type in new[] { RegistrationType.OS32Bit, RegistrationType.OS64Bit })
                {
                    ServerRegistrationManager.UnregisterServer(serverType, type);
                    ServerRegistrationManager.UninstallServer(serverType, type);
                }
        }

        static IEnumerable<ISharpShellServer> LoadServerTypes(string assemblyPath)
        {
            var container = new CompositionContainer(new AssemblyCatalog(assemblyPath));
            return container.GetExports<ISharpShellServer>().Select(x => x.Value);
        }
    }
}
