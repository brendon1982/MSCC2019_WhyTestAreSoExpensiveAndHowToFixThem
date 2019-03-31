using System.Collections.Generic;
using System.Linq;
using Ls.Domain.Tests.Builders;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using static NExpect.Expectations;
using PeanutButter.RandomGenerators;

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
                Expect(actualFiles).To.Deep.Equal(files);
            }

            [Test]
            public void WhenFilesNotInAlphabeticalOrder_ShouldRespondWithTheFilesInAlphabeticalOrder()
            {
                // Arrange
                var path = "D:\\somewhere";

                var runExe = FsFileTestDataBuilder.Create().WithName("run.exe").Build();
                var yayDocx = FsFileTestDataBuilder.Create().WithName("yay.docx").Build();
                var cakeTxt = FsFileTestDataBuilder.Create().WithName("cake.txt").Build();
                var isYummyXlsx = FsFileTestDataBuilder.Create().WithName("isYummy.xlsx").Build();
                var files = new List<FsFile>
                {
                    runExe, yayDocx, cakeTxt, isYummyXlsx
                };

                List<IFsItem> actualFiles = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFiles = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, files);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Expect(actualFiles).To.Deep.Equal(new List<FsFile>
                {
                    cakeTxt, isYummyXlsx, runExe, yayDocx
                });
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
                Expect(actualDirectories).To.Deep.Equal(directories);
            }

            [Test]
            public void WhenFilesNotInAlphabeticalOrder_ShouldRespondWithTheDirectoriesInAlphabeticalOrder()
            {
                // Arrange
                var path = "X:\\somewhere_else";

                var notes = FsDirectoryTestDataBuilder.Create().WithName("notes").Build();
                var zebras = FsDirectoryTestDataBuilder.Create().WithName("zebras").Build();
                var memes = FsDirectoryTestDataBuilder.Create().WithName("memes").Build();
                var books = FsDirectoryTestDataBuilder.Create().WithName("books").Build();
                var directories = new List<FsDirectory>
                {
                    notes, zebras, memes, books
                };

                List<IFsItem> actualDirectories = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualDirectories = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Expect(actualDirectories).To.Deep.Equal(new List<IFsItem>
                {
                    books, memes, notes, zebras
                });
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

                var stuffExeFile = FsFileTestDataBuilder.Create().WithName("stuff.exe").Build();
                var gamesExeFile = FsFileTestDataBuilder.Create().WithName("games.exe").Build();
                var files = new List<FsFile>
                {
                    stuffExeFile, gamesExeFile
                };

                var unconferenceDirectory = FsDirectoryTestDataBuilder.Create().WithName("unconference").Build();
                var conferenceDirectory = FsDirectoryTestDataBuilder.Create().WithName("conference").Build();
                var directories = new List<FsDirectory>
                {
                    unconferenceDirectory, conferenceDirectory
                };

                List<IFsItem> actualFsItems = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithFsItemsSnapshot(f => actualFsItems = f)
                    .Build();

                var lsUseCase = CreateLsUseCase(path, files, directories);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Expect(actualFsItems).To.Deep.Equal(new List<IFsItem>
                {
                    conferenceDirectory, gamesExeFile, stuffExeFile, unconferenceDirectory
                });
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
                Expect(actualFsItems).To.Be.Empty();
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

        [TestFixture]
        public class Errors
        {
            [Test]
            public void WhenThereIsAnErrorReadingFiles_ShouldRespondWithThatError()
            {
                // Arrange
                var path = "E:\\";
                var error = RandomValueGen.GetRandomString();

                string actualError = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithErrorSnapshot(e => actualError = e)
                    .Build();

                var lsUseCase = CreateLsUseCaseWithFilesError(path, error);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Expect(actualError).To.Be.Equal.To(error);
            }

            [Test]
            public void WhenThereIsAnErrorReadingDirectories_ShouldRespondWithThatError()
            {
                // Arrange
                var path = "R:\\";
                var error = RandomValueGen.GetRandomString();

                string actualError = null;
                var presenter = SubstituteFsItemPresenterBuilder.Create()
                    .WithErrorSnapshot(e => actualError = e)
                    .Build();

                var lsUseCase = CreateLsUseCaseWithDirectoriesError(path, error);
                // Act
                lsUseCase.Execute(path, presenter);
                // Assert
                Expect(actualError).To.Be.Equal.To(error);
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

        private static LsUseCase CreateLsUseCaseWithFilesError(string path, string error)
        {
            var filesGateway = SubstituteFilesGatewayBuilder.Create()
                .WithError(path, error)
                .Build();

            var directoriesGateway = SubstituteDirectoriesGatewayBuilder.Create()
                .Build();

            return new LsUseCase(filesGateway, directoriesGateway, Substitute.For<ILog>());
        }

        private static LsUseCase CreateLsUseCaseWithDirectoriesError(string path, string error)
        {
            var filesGateway = SubstituteFilesGatewayBuilder.Create()
                .Build();

            var directoriesGateway = SubstituteDirectoriesGatewayBuilder.Create()
                .WithError(path, error)
                .Build();

            return new LsUseCase(filesGateway, directoriesGateway, Substitute.For<ILog>());
        }

    }
}