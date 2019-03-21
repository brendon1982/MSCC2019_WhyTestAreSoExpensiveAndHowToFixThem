using System.Collections.Generic;
using System.Linq;

namespace Ls.Domain
{
    public class LsUseCase
    {
        private readonly IFileSystemGateway _fileSystemGateway;

        public LsUseCase(IFileSystemGateway fileSystemGateway)
        {
            _fileSystemGateway = fileSystemGateway;
        }

        public void Execute(string path, IFsItemPresenter presenter)
        {
            var files = _fileSystemGateway
                .Files(path)
                .OrderBy(file => file.Name);

            presenter.Respond(files);
        }
    }
}