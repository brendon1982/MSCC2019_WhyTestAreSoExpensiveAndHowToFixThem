using System.Linq;

namespace Ls.Domain
{
    public class FsFile: IFsItem
    {
        public FsFile(string name)
        {
            Name = name;
            Extension = GetExtension(name);
        }

        private string GetExtension(string name)
        {
            if (name.Contains('.'))
            {
                return name.Reverse().TakeWhile(c => c != '.').Reverse().ToString();
            }

            return "";
        }

        public string Name { get; }
        public string Extension { get; set; }
    }
}