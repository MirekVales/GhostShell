using System.IO;
using System.Linq;

namespace GhostShell.Behaviour
{
    public class AssociationPredicate
    {
        public ObjectTypes Type { get; set; }
        public string[] AllowedFileExtensions { get; set; }

        public bool Ok(SelectedItems filter)
        {
            if (Type.HasFlag(ObjectTypes.File) && filter.Files.Any() && IsExtensionConstraintOk(filter.Files))
                return true;
            if (Type.HasFlag(ObjectTypes.Directory) && filter.Directories.Any())
                return true;

            return false;
        }

        public bool IsExtensionConstraintOk(string[] paths)
        {
            if (AllowedFileExtensions == null || AllowedFileExtensions.Length == 0)
                return true;

            if (AllowedFileExtensions.Contains("*.*"))
                return true;

            return paths.Any(x => AllowedFileExtensions.Contains(Path.GetExtension(x).ToLower()));
        }
    }
}
