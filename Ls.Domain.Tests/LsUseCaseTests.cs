using System.Collections.Generic;
using System.Linq;
using Ls.Domain.Tests.Builders;
using NExpect;
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

                List<IFsItem> actualFiles = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFiles = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, files);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualFiles[0].Name, Is.EqualTo("a.txt"));
                Assert.That(actualFiles[1].Name, Is.EqualTo("b.exe"));
                Assert.That(actualFiles[2].Name, Is.EqualTo("c.dat"));
                Assert.That(actualFiles.Count, Is.EqualTo(3));
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

                List<IFsItem> actualFiles = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFiles = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, files);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualFiles[0].Name, Is.EqualTo("cake.txt"));
                Assert.That(actualFiles[1].Name, Is.EqualTo("isYummy.xlsx"));
                Assert.That(actualFiles[2].Name, Is.EqualTo("run.exe"));
                Assert.That(actualFiles[3].Name, Is.EqualTo("yay.docx"));
                Assert.That(actualFiles.Count, Is.EqualTo(4));
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

                List<IFsItem> actualDirectories = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualDirectories = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualDirectories[0].Name, Is.EqualTo("cake recipes"));
                Assert.That(actualDirectories[1].Name, Is.EqualTo("code"));
                Assert.That(actualDirectories[2].Name, Is.EqualTo("talks"));
                Assert.That(actualDirectories.Count, Is.EqualTo(3));
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

                List<IFsItem> actualDirectories = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualDirectories = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualDirectories[0].Name, Is.EqualTo("books"));
                Assert.That(actualDirectories[1].Name, Is.EqualTo("memes"));
                Assert.That(actualDirectories[2].Name, Is.EqualTo("notes"));
                Assert.That(actualDirectories[3].Name, Is.EqualTo("zebras"));
                Assert.That(actualDirectories.Count, Is.EqualTo(4));
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

                List<IFsItem> actualFsItems = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFsItems = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, files, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualFsItems[0].Name, Is.EqualTo("conference"));
                Assert.That(actualFsItems[1].Name, Is.EqualTo("games.exe"));
                Assert.That(actualFsItems[2].Name, Is.EqualTo("stuff.exe"));
                Assert.That(actualFsItems[3].Name, Is.EqualTo("unconference"));
                Assert.That(actualFsItems.Count, Is.EqualTo(4));
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

                List<IFsItem> actualFsItems = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFsItems = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, new List<FsFile>(), new List<FsDirectory>());
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Assert.That(actualFsItems.Count, Is.EqualTo(0));
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

        private static LsUseCase CreateLsUseCase(string path, List<FsFile> files)
        {
            return CreateLsUseCase(path, files, new List<FsDirectory>());
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsDirectory> directories)
        {
            return CreateLsUseCase(path, new List<FsFile>(), directories);
        }

        private static LsUseCase CreateLsUseCase(string path, List<FsFile> files, List<FsDirectory> directories)
        {
            var filesGateway = SubstituteFilesGatewayBuilder.Create()
                .WithFiles(path, files)
                .Build();

            var directoriesGateway = SubstituteDirectoriesGatewayBuilder.Create()
                .WithDirectories(path, directories)
                .Build();

            return new LsUseCase(filesGateway, directoriesGateway, Substitute.For<ILog>());
        }

        private static LsUseCase CreateLsUseCase(ILog log)
        {
            var filesGateway = SubstituteFilesGatewayBuilder.Create()
                .Build();

            var directoriesGateway = SubstituteDirectoriesGatewayBuilder.Create()
                .Build();

            return new LsUseCase(filesGateway, directoriesGateway, log);
        }

    }
}