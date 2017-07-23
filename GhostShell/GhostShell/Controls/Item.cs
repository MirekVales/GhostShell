using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GhostShell.Controls
{
    [DataContract]
    [KnownType(typeof(Button))]
    [KnownType(typeof(Label))]
    [KnownType(typeof(MenuItem))]
    [KnownType(typeof(Separator))]
    public abstract class Item
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Item[] Items { get; set; }

        public Action Action { get; protected set; }

        public IEnumerable<Item> Descendants()
        {
            if (Items == null)
                yield break;

            foreach (var item in Items)
            {
                foreach (var descendant in item.Descendants())
                    yield return descendant;
                yield return item;
            }
        }

        public Item()
            => Id = Guid.NewGuid();

        public abstract ToolStripItem Construct(Func<Guid, bool> executionHandler);
    }
}
