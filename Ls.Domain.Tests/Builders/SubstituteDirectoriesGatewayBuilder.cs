using System.Collections.Generic;
using NSubstitute;

namespace Ls.Domain.Tests.Builders
{
    public class SubstituteDirectoriesGatewayBuilder
    {
        private readonly Dictionary<string, List<FsDirectory>> _directories = new Dictionary<string, List<FsDirectory>>();

        public static SubstituteDirectoriesGatewayBuilder Create()
        {
            return new SubstituteDirectoriesGatewayBuilder();
        }

        public SubstituteDirectoriesGatewayBuilder WithDirectories(string path, List<FsDirectory> directories)
        {
            _directories[path] = directories;
            return this;
        }

        public IDirectoriesGateway Build()
        {
            var directoriesGateway = Substitute.For<IDirectoriesGateway>();

            foreach (var directory in _directories)
            {
                directoriesGateway.Directories(directory.Key).Returns(directory.Value);
            }

            return directoriesGateway;
        }
    }
}