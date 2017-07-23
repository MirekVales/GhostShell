using System.Linq;
using System.Collections.Generic;
using GhostShell.Controls;

namespace GhostShell.Behaviour
{
    public class Association
    {
        public static Association Create => new Association();

        readonly HashSet<string> fileExtensions = new HashSet<string>();
        readonly HashSet<Item> items = new HashSet<Item>();

        internal ObjectTypes AssociatedType { get; private set; }

        public string[] FileExtensions => fileExtensions.ToArray();
        public Item[] Item => items.ToArray();

        internal AssociationPredicate AssociationPredicate
            => new AssociationPredicate() {
                Type = AssociatedType,
                AllowedFileExtensions = FileExtensions
            };

        public Association AssociatedToFile(params string[] extensions)
        {
            AssociatedType |= ObjectTypes.File;
            foreach (var extension in extensions)
                fileExtensions.Add(extension.ToLower());
            return this;
        }

        public Association AssociatedToDirectory()
        {
            AssociatedType |= ObjectTypes.Directory;
            return this;
        }

        public Association WithItem(params Item[] items)
        {
            foreach (var item in items)
                this.items.Add(item);
            return this;
        }
    }
}
