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
                .OfType<IFsItem>()
                .OrderBy(file => file.Name);

            var directories = _fileSystemGateway
                .Directories(path)
                .OfType<IFsItem>()
                .OrderBy(file => file.Name);

            var fsItems = files
                .Concat(directories);

            presenter.Respond(fsItems);
        }
    }
}