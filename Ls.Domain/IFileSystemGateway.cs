using System.Collections.Generic;

namespace Ls.Domain
{
    public interface IFileSystemGateway
    {
        IEnumerable<FsFile> Files(string path);
        IEnumerable<FsDirectory> Directories(string path);
    }
}