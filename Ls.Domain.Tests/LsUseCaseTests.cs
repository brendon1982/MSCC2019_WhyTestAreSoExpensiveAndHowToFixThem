using System.Collections.Generic;
using System.Linq;
using Ls.Domain.Tests.Builders;
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
                var files = new List<FsFile>
                {
                    FsFileTestDataBuilder.Create().WithName("a.txt").Build(),
                    FsFileTestDataBuilder.Create().WithName("b.exe").Build(),
                    FsFileTestDataBuilder.Create().WithName("c.dat").Build()
                };

                var presenter = Substitute.For<IFsItemPresenter>();

                var lsUseCase = CreateLsUseCase(path, files);
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
                var files = new List<FsFile>
                {
                    FsFileTestDataBuilder.Create().WithName("run.exe").Build(),
                    FsFileTestDataBuilder.Create().WithName("yay.docx").Build(),
                    FsFileTestDataBuilder.Create().WithName("cake.txt").Build(),
                    FsFileTestDataBuilder.Create().WithName("isYummy.xlsx").Build()
                };

                var presenter = Substitute.For<IFsItemPresenter>();

                var lsUseCase = CreateLsUseCase(path, files);
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
                var directories = new List<FsDirectory>{
                    FsDirectoryTestDataBuilder.Create().WithName("cake recipes").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("code").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("talks").Build()
                };

                var presenter = Substitute.For<IFsItemPresenter>();

                var lsUseCase = CreateLsUseCase(path, directories);
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
                var directories = new List<FsDirectory>{
                    FsDirectoryTestDataBuilder.Create().WithName("notes").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("zebras").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("memes").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("books").Build()
                };

                var presenter = Substitute.For<IFsItemPresenter>();

                var lsUseCase = CreateLsUseCase(path, directories);
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
                var files = new List<FsFile>{
                    FsFileTestDataBuilder.Create().WithName("stuff.exe").Build(),
                    FsFileTestDataBuilder.Create().WithName("games.exe").Build()
                };
                var directories = new List<FsDirectory>{
                    FsDirectoryTestDataBuilder.Create().WithName("unconference").Build(),
                    FsDirectoryTestDataBuilder.Create().WithName("conference").Build()
                };

                var presenter = Substitute.For<IFsItemPresenter>();

                var lsUseCase = CreateLsUseCase(path, files, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    fsItems.Count() == 4 &&
                    fsItems.ElementAt(0).Name == "conference" &&
                    fsItems.ElementAt(1).Name == "games.exe" &&
                    fsItems.ElementAt(2).Name == "stuff.exe" &&
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

                var lsUseCase = CreateLsUseCase(path, new List<FsFile>(), new List<FsDirectory>());
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                presenter.Received().Respond(Arg.Is<IEnumerable<IFsItem>>(fsItems =>
                    !fsItems.Any()
                ));
            }
        }
        
        [TestFixture]
        public class Logging
        {
            [Test]
            public void ShouldWritePathToLog()
            {
                // Arrange
                var path = "E:\\";

                var presenter = Substitute.For<IFsItemPresenter>();

                var log = Substitute.For<ILog>();

                var lsUseCase = CreateLsUseCase(log);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                log.Received().Info(path);
            }
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsFile> files, List<FsDirectory> directories)
        {
            return CreateLsUseCase(path, files, directories, Substitute.For<ILog>());
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsFile> files)
        {
            return CreateLsUseCase(path, files, new List<FsDirectory>(), Substitute.For<ILog>());
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsDirectory> directories)
        {
            return CreateLsUseCase(path, new List<FsFile>(), directories, Substitute.For<ILog>());
        }

        private static LsUseCase CreateLsUseCase(ILog log)
        {
            return CreateLsUseCase("C:\\", new List<FsFile>(), new List<FsDirectory>(), log);
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsFile> files, List<FsDirectory> directories, ILog log)
        {
            var filesGateway = Substitute.For<IFilesGateway>();
            filesGateway.Files(path).Returns(files);

            var directoriesGateway = Substitute.For<IDirectoriesGateway>();
            directoriesGateway.Directories(path).Returns(directories);

            return new LsUseCase(filesGateway, directoriesGateway, log);
        }
    }
}