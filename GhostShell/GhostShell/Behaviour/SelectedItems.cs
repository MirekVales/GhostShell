using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GhostShell.Behaviour
{
    [DataContract]
    public class SelectedItems
    {
        [DataMember]
        public string[] Files { get; set; }
        [DataMember]
        public string[] Extensions { get; set; }
        [DataMember]
        public string[] Directories { get; set; }

        public SelectedItems()
        { }

        public SelectedItems(IEnumerable<string> selectedPaths)
        {
            var files = new HashSet<string>();
            var extensions = new HashSet<string>();
            var directories = new HashSet<string>();

            foreach (var path in selectedPaths)
            {
                try
                {
                    var attr = File.GetAttributes(path);
                    if (attr.HasFlag(FileAttributes.Directory))
                        directories.Add(path);
                    else
                    {
                        files.Add(path);
                        extensions.Add(Path.GetExtension(path).ToLower());
                    }
                }
                catch { }
            }

            Files = files.ToArray();
            Extensions = extensions.ToArray();
            Directories = directories.ToArray();
        }
    }
}
