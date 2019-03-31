using System.Collections.Generic;
using NSubstitute;

namespace Ls.Domain.Tests.Builders
{
    public class SubstituteFilesGatewayBuilder
    {
        private readonly Dictionary<string, List<FsFile>> _files = new Dictionary<string, List<FsFile>>();

        public static SubstituteFilesGatewayBuilder Create()
        {
            return new SubstituteFilesGatewayBuilder();
        }

        public SubstituteFilesGatewayBuilder WithFiles(string path, List<FsFile> _Files)
        {
            _files[path] = _Files;
            return this;
        }

        public IFilesGateway Build()
        {
            var filesGateway = Substitute.For<IFilesGateway>();

            foreach (var directory in _files)
            {
                filesGateway.Files(directory.Key).Returns(directory.Value);
            }

            return filesGateway;
        }
    }
}