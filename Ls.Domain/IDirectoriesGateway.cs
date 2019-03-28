using System.Collections.Generic;

namespace Ls.Domain
{
    public interface IDirectoriesGateway
    {
        IEnumerable<FsDirectory> Directories(string path);
    }
}