using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using NSubstitute;

namespace DevFreela.UnitTests.Application;

public class InsertProjectHandlerTests
{
    [Fact]
    public async Task InputDataAreOk_Insert_Success_NSubstitute()
    {
        // Arrange
        const int ID = 1;
        var repository = Substitute.For<IProjectRepository>();
        repository.Add(Arg.Any<Project>(), CancellationToken.None).Returns(Task.FromResult(ID));
        
        var command = new InsertProjectCommand
        {
            Title = "New Project",
            Description = "Project Description",
            IdClient = 1,
            IdFreelancer = 2,
            TotalCost = 1000.00m
        };
        
        var handler = new InsertProjectHandler(repository);
        
        // Act
        
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ID, result.Data);
        await repository.Received(1).Add(Arg.Any<Project>(), CancellationToken.None);
    }
    
    [Fact]
    public async Task InputDataAreOk_Insert_Success_Moq()
    {
        // Arrange
        const int ID = 1;
        
        // var mock = new Mock<IProjectRepository>();
        // mock.Setup(repo => repo.Add(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
        //     .ReturnsAsync(ID);
        
        var repository = Mock
            .Of<IProjectRepository>(r => r.Add(It.IsAny<Project>(), It.IsAny<CancellationToken>()) == Task.FromResult(ID));
        
        var command = new InsertProjectCommand
        {
            Title = "New Project",
            Description = "Project Description",
            IdClient = 1,
            IdFreelancer = 2,
            TotalCost = 1000.00m
        };
        
        var handler = new InsertProjectHandler(repository);
        
        // Act
        
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ID, result.Data);
        //mock.Verify(repo => repo.Add(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
        Mock.Get(repository).Verify(repo => repo.Add(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}