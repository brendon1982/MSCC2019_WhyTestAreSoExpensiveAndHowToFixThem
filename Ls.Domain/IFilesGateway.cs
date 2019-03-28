using System.Collections.Generic;

namespace Ls.Domain
{
    public interface IFilesGateway
    {
        IEnumerable<FsFile> Files(string path);
    }
}