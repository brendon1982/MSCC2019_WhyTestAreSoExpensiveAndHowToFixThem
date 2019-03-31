using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Ls.Domain.Tests.Builders
{
    public class SubstituteDirectoriesGatewayBuilder
    {
        private readonly Dictionary<string, List<FsDirectory>> _directories = new Dictionary<string, List<FsDirectory>>();
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public static SubstituteDirectoriesGatewayBuilder Create()
        {
            return new SubstituteDirectoriesGatewayBuilder();
        }

        public SubstituteDirectoriesGatewayBuilder WithDirectories(string path, List<FsDirectory> directories)
        {
            _directories[path] = directories;
            return this;
        }

        public SubstituteDirectoriesGatewayBuilder WithError(string path, string error)
        {
            _errors[path] = error;
            return this;
        }

        public IDirectoriesGateway Build()
        {
            var directoriesGateway = Substitute.For<IDirectoriesGateway>();

            foreach (var directory in _directories)
            {
                directoriesGateway.Directories(directory.Key).Returns(directory.Value);
            }

            foreach (var error in _errors)
            {
                directoriesGateway.Directories(error.Key).Throws(new Exception(error.Value));
            }

            return directoriesGateway;
        }
    }
}