using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GhostShell.Controls;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace GhostShell.Client
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.Drive)]
    public class GhostShellClient : SharpContextMenu
    {
        Item[] items;

        protected override bool CanShowMenu()
            => (items = WcfClient.GetMenuItems(SelectedItemPaths)).Any();

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            var toolStripItems = items.Select(item => item.Construct(WcfClient.Execute));
            menu.Items.AddRange(toolStripItems.ToArray());
            return menu;
        }
    }
}
