namespace Ls.Domain
{
    public class FsDirectory : IFsItem
    {
        public FsDirectory(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}