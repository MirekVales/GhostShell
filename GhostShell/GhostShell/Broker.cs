using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ServiceModel;
using GhostShell.Controls;
using GhostShell.Behaviour;

namespace GhostShell
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Broker : IBroker
    {
        readonly Dictionary<Guid, Action> operations
            = new Dictionary<Guid, Action>();

        readonly Dictionary<AssociationPredicate, Item[]> registrations
            = new Dictionary<AssociationPredicate, Item[]>();

        public bool ExecuteOperation(Guid id)
        {
            if (operations.ContainsKey(id))
            {
                Task.Run(() => { operations[id](); });
                return true;
            }
            return false;
        }

        public Item[] GetMenuItems(SelectedItems items)
            =>  registrations
                    .Where(x => x.Key.Ok(items))
                    .SelectMany(x => x.Value)
                    .ToArray();

        public void RegisterExpression(params Association[] expressions)
        {
            foreach (var expression in expressions)
                registrations.Add(expression.AssociationPredicate, expression.Item);

            expressions
                .SelectMany(x => x.Item.SelectMany(y => y.Descendants()))
                .Where(x => x.Action != null && operations.ContainsKey(x.Id))
                .ToList()
                .ForEach(x => operations.Add(x.Id, x.Action));
        }
    }
}
