using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Ls.Domain.Tests
{
    // Notes
    //  1) simple LS command
    //  2) clean architecture (interactors/use cases, gateways, entities, requests & responses)
    //  3) just enough tests to show implementation knowledge,
    //     I tried to get a balance between something real but still understandable
    //  4) mixture of london style TDD & classic TDD (starts off london style)
    //     london style = heavy interaction testing (roles & responsibilities)
    //     classic      = more state based (algorithms)

    [TestFixture]
    public class LsUseCaseTests
    {
        [TestFixture]
        public class OnlyFilesInPath
        {
            [Test]
            public void GivenFilesInAlphabeticalOrder_ShouldRespondWithTheFiles()
            {
                // Arrange
                var path = "C:\\";

                var presenter = Substitute.For<IPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>
                {
                    new FsFile { Name = "a.txt" },
                    new FsFile { Name = "b.exe" },
                    new FsFile { Name = "c.dat" },
                });
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>());
                
                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<List<IFsItem>>(fsItems => 
                    fsItems.Count == 3 && 
                    fsItems[0].Name == "a.txt" &&
                    fsItems[1].Name == "b.exe" &&
                    fsItems[2].Name == "c.dat"
                ));
            }
        }

        // TODO Only directories in path
        // TODO Nothing in path
        // TODO alphabetical order
    }
}