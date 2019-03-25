namespace Ls.Domain.Tests.Builders
{
    public class FsFileTestDataBuilder
    {
        private string _name;

        public static FsFileTestDataBuilder Create()
        {
            return new FsFileTestDataBuilder();
        }

        public FsFileTestDataBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public FsFile Build()
        {
            return new FsFile(_name);
        }
    }
}