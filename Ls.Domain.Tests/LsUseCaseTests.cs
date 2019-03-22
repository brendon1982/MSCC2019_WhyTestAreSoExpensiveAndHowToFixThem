using System;
using System.Collections.Generic;
using System.Linq;
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
            public void ShouldRespondWithTheFiles()
            {
                // Arrange
                var path = "C:\\";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>
                {
                    new FsFile { Name = "a.txt" },
                    new FsFile { Name = "b.exe" },
                    new FsFile { Name = "c.dat" }
                });
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>());
                
                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems => 
                    fsItems.Count() == 3 && 
                    fsItems.ElementAt(0).Name == "a.txt" &&
                    fsItems.ElementAt(1).Name == "b.exe" &&
                    fsItems.ElementAt(2).Name == "c.dat"
                ));
            }

            [Test]
            public void WhenFilesNotInAlphabeticalOrder_ShouldRespondWithTheFilesInAlphabeticalOrder()
            {
                // Arrange
                var path = "D:\\somewhere";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>
                {
                    new FsFile { Name = "run.exe" },
                    new FsFile { Name = "yay.docx" },
                    new FsFile { Name = "cake.txt" },
                    new FsFile { Name = "isYummy.xlsx" }
                });
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>());

                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    fsItems.Count() == 4 &&
                    fsItems.ElementAt(0).Name == "cake.txt" &&
                    fsItems.ElementAt(1).Name == "isYummy.xlsx" &&
                    fsItems.ElementAt(2).Name == "run.exe" &&
                    fsItems.ElementAt(3).Name == "yay.docx"
                ));
            }
        }

        [TestFixture]
        public class OnlyDirectoriesInPath
        {
            [Test]
            public void ShouldRespondWithTheDirectories()
            {
                // Arrange
                var path = "Z:\\";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>());
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>{
                    new FsDirectory { Name = "cake recipes" },
                    new FsDirectory { Name = "code" },
                    new FsDirectory { Name = "talks" }
                });

                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    fsItems.Count() == 3 &&
                    fsItems.ElementAt(0).Name == "cake recipes" &&
                    fsItems.ElementAt(1).Name == "code" &&
                    fsItems.ElementAt(2).Name == "talks"
                ));
            }

            [Test]
            public void WhenFilesNotInAlphabeticalOrder_ShouldRespondWithTheDirectoriesInAlphabeticalOrder()
            {
                // Arrange
                var path = "X:\\somewhere_else";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>());
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>{
                    new FsDirectory { Name = "notes" },
                    new FsDirectory { Name = "zebras" },
                    new FsDirectory { Name = "memes" },
                    new FsDirectory { Name = "books" }
                });

                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    fsItems.Count() == 4 &&
                    fsItems.ElementAt(0).Name == "books" &&
                    fsItems.ElementAt(1).Name == "memes" &&
                    fsItems.ElementAt(2).Name == "notes" &&
                    fsItems.ElementAt(3).Name == "zebras"
                ));
            }
        }

        [TestFixture]
        public class FilesAndDirectoriesInPath
        {
            [Test]
            public void ShouldRespondWithFilesAndDirectoriesInAlphabeticalOrder()
            {
                // Arrange
                var path = "M:\\documents";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>{
                    new FsFile { Name = "stuff.txt" },
                    new FsFile { Name = "games.exe" }
                });
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>{
                    new FsDirectory { Name = "unconference" },
                    new FsDirectory { Name = "conference" }
                });

                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    fsItems.Count() == 4 &&
                    fsItems.ElementAt(0).Name == "conference" &&
                    fsItems.ElementAt(1).Name == "games.exe" &&
                    fsItems.ElementAt(2).Name == "stuff.txt" &&
                    fsItems.ElementAt(3).Name == "unconference"
                ));
            }
        }

        [TestFixture]
        public class NoFilesAndNoDirectoriesInPath
        {
            [Test]
            public void ShouldRespondWithNoFsItems()
            {
                // Arrange
                var path = "E:\\";

                var presenter = Substitute.For<IFsItemPresenter>();

                var fileSystemGateway = Substitute.For<IFileSystemGateway>();
                fileSystemGateway.Files(path).Returns(new List<FsFile>());
                fileSystemGateway.Directories(path).Returns(new List<FsDirectory>());

                var lsUseCase = new LsUseCase(fileSystemGateway);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    !fsItems.Any()
                ));
            }
        }

        // TODO Only directories in path
    }
}