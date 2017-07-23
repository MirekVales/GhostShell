using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace GhostShell.Controls
{
    [DataContract]
    public class MenuItem : Item
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public byte[] IconData { get; set; }

        Image IconBitmap
            => IconData == null ? null : Image.FromStream(new MemoryStream(IconData));

        public MenuItem(string text, params Item[] items)
        {
            Text = text;
            Items = items;
        }

        public MenuItem(string text, Image bitmap, params Item[] items)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                IconData = stream.ToArray();
            }
            Text = text;
            Items = items;
        }

        public MenuItem(string text, Image bitmap, Action action, params Item[] items)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                IconData = stream.ToArray();
            }
            Text = text;
            Items = items;
            Action = action;
        }

        public override ToolStripItem Construct(Func<Guid, bool> executionHandler)
        {
            var item = new ToolStripMenuItem(Text, IconBitmap, (s, a) => executionHandler(Id));
            foreach (var subItem in Items)
                item.DropDownItems.Add(subItem.Construct(executionHandler));
            return item;
        }
    }
}
