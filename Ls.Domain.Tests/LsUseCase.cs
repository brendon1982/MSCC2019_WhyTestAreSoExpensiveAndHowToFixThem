using System.Collections.Generic;

namespace Ls.Domain.Tests
{
    public class LsUseCase
    {
        private readonly IFileSystemGateway _fileSystemGateway;

        public LsUseCase(IFileSystemGateway fileSystemGateway)
        {
            _fileSystemGateway = fileSystemGateway;
        }

        public void Execute(string path, IPresenter presenter)
        {
            presenter.Respond(new List<IFsItem>
            {
                new FsFile { Name = "a.txt"},
                new FsFile { Name = "b.exe"},
                new FsFile { Name = "c.dat"}
            });
        }
    }
}