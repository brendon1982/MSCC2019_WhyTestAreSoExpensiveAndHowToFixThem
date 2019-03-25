namespace Ls.Domain.Tests.Builders
{
    public class FsDirectoryTestDataBuilder
    {
        private string _name;

        public static FsDirectoryTestDataBuilder Create()
        {
            return new FsDirectoryTestDataBuilder();
        }

        public FsDirectoryTestDataBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public FsDirectory Build()
        {
            return new FsDirectory
            {
                Name = _name
            };
        }
    }
}