using System;

namespace GhostShell.Behaviour
{
    [Flags]
    public enum ObjectTypes
    {
        None = 0,
        File = 1, 
        Directory = 2,
        Drive = 4
    }
}
