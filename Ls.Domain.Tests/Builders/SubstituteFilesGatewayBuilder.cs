using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Ls.Domain.Tests.Builders
{
    public class SubstituteFilesGatewayBuilder
    {
        private readonly Dictionary<string, List<FsFile>> _files = new Dictionary<string, List<FsFile>>();
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        public static SubstituteFilesGatewayBuilder Create()
        {
            return new SubstituteFilesGatewayBuilder();
        }

        public SubstituteFilesGatewayBuilder WithFiles(string path, List<FsFile> _Files)
        {
            _files[path] = _Files;
            return this;
        }

        public SubstituteFilesGatewayBuilder WithError(string path, string error)
        {
            _errors[path] = error;
            return this;
        }


        public IFilesGateway Build()
        {
            var filesGateway = Substitute.For<IFilesGateway>();

            foreach (var directory in _files)
            {
                filesGateway.Files(directory.Key).Returns(directory.Value);
            }

            foreach (var error in _errors)
            {
                filesGateway.Files(error.Key).Throws(new Exception(error.Value));
            }

            return filesGateway;
        }
    }
}