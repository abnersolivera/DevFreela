using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using FluentAssertions;

namespace DevFreela.UnitTests.Core;

public class ProjectTests
{
    [Fact]
    public void ProjectIsCreated_Start_Success()
    {
        // Arrange
        var project = new Project("Projeto A", "Descrição do projeto", 1, 2, 1000);
        
        // Act
        
        project.Start();
        
        // Assert
        Assert.Equal(EProjectStatus.InProgress, project.Status);
        project.Status.Should().Be(EProjectStatus.InProgress);
        Assert.NotNull(project.StartedAt);
        project.StartedAt.Should().NotBeNull();
        
        Assert.True(project.Status == EProjectStatus.InProgress);
        
        Assert.False(project.StartedAt is null);
    }
    
    [Fact]
    public void ProjectIsInInvalidState_Start_ThrowsException()
    {
        // Arrange
        var project = new Project("Projeto A", "Descrição do projeto", 1, 2, 1000);
        project.Start();
        
        // Act + Assert
        
        Action start = project.Start;
        var exception = Assert.Throws<InvalidOperationException>(start);
        Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);
        start.Should().Throw<InvalidOperationException>().WithMessage(Project.INVALID_STATE_MESSAGE);
    }
}