using System.Collections.Generic;

namespace Ls.Domain.Tests
{
    public interface IFileSystemGateway
    {
        IEnumerable<FsFile> Files(string path);
        IEnumerable<FsDirectory> Directories(string path);
    }
}