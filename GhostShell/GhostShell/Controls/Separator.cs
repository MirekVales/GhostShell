using System;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace GhostShell.Controls
{
    [DataContract]
    public class Separator : Item
    {
        public override ToolStripItem Construct(Func<Guid, bool> executionHandler)
            => new ToolStripSeparator();
    }
}
