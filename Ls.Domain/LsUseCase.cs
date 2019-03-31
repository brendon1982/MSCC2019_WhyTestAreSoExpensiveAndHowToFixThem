using System;
using System.Collections.Generic;
using System.Linq;

namespace Ls.Domain
{
    public class LsUseCase
    {
        private readonly IFilesGateway _filesGateway;
        private readonly ILog _log;
        private readonly IDirectoriesGateway _directoriesGateway;

        public LsUseCase(IFilesGateway filesGateway, IDirectoriesGateway directoriesGateway, ILog log)
        {
            _directoriesGateway = directoriesGateway;
            _log = log;
            _filesGateway = filesGateway;
        }

        public void Execute(string path, IFsItemPresenter presenter)
        {
            _log.Info(path);

            try
            {
                presenter.Respond(OrderedFilesAndDirectories(path), null);
            }
            catch (Exception e)
            {
                presenter.Respond(new List<IFsItem>(), e.Message);
            }
        }

        private IOrderedEnumerable<IFsItem> OrderedFilesAndDirectories(string path)
        {
            var files = _filesGateway
                .Files(path)
                .OfType<IFsItem>();

            var directories = _directoriesGateway
                .Directories(path)
                .OfType<IFsItem>();

            var fsItems = files
                .Concat(directories)
                .OrderBy(item => item.Name);

            return fsItems;
        }
    }
}