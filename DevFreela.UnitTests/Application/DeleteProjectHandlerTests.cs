using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using FluentAssertions;
using Moq;
using NSubstitute;

namespace DevFreela.UnitTests.Application;

public class DeleteProjectHandlerTests
{
    [Fact]
    public async Task ProjectExists_Delete_Success_NSubstitute()
    {
        // Arrange
        const int ID = 1;
        var project = new Project("Test Project", "Project Description", 1, 2, 1000.00m);
        var repository = Substitute.For<IProjectRepository>();
        repository.GetById(ID, CancellationToken.None).Returns(Task.FromResult((Project?)project));
        repository.Update(Arg.Any<Project>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        var command = new DeleteProjectCommand(ID);
        var handler = new DeleteProjectHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await repository.Received(1).GetById(ID, CancellationToken.None);
        await repository.Received(1).Update(Arg.Any<Project>(), CancellationToken.None);
    }

    [Fact]
    public async Task ProjectDoesNotExist_Delete_Error_NSubstitute()
    {
        // Arrange
        const int ID = 1;
        var repository = Substitute.For<IProjectRepository>();
        repository.GetById(ID, CancellationToken.None).Returns(Task.FromResult((Project?)null));
        var command = new DeleteProjectCommand(ID);
        var handler = new DeleteProjectHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Project not found", result.Message);
        await repository.Received(1).GetById(ID, CancellationToken.None);
        await repository.DidNotReceive().Update(Arg.Any<Project>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProjectExists_Delete_Success_Moq()
    {
        // Arrange
        const int ID = 1;
        var project = new Project("Test Project", "Project Description", 1, 2, 1000.00m);
        var repository = Mock.Of<IProjectRepository>(p =>
            p.GetById(ID, CancellationToken.None) == Task.FromResult(project) &&
            p.Update(It.IsAny<Project>(), It.IsAny<CancellationToken>()) == Task.CompletedTask);

        var command = new DeleteProjectCommand(ID);
        var handler = new DeleteProjectHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Mock.Get(repository).Verify(r => r.GetById(ID, CancellationToken.None), Times.Once);
        Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProjectDoesNotExist_Delete_Error_Moq()
    {
        // Arrange
        const int ID = 1;
        var repository = Mock.Of<IProjectRepository>(p => p.GetById(ID, CancellationToken.None) == Task.FromResult((Project?)null));
        var command = new DeleteProjectCommand(ID);
        var handler = new DeleteProjectHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        result.IsSuccess.Should().BeFalse();
        Assert.Equal("Project not found", result.Message);
        Mock.Get(repository).Verify(r => r.GetById(ID, CancellationToken.None), Times.Once);
        Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}