using System;
using System.ServiceModel;
using GhostShell.Controls;
using GhostShell.Behaviour;

namespace GhostShell
{
    [ServiceContract]
    public interface IBroker
    {
        [OperationContract]
        Item[] GetMenuItems(SelectedItems filter);

        [OperationContract]
        bool ExecuteOperation(Guid id);
    }
}
