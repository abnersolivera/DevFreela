using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultViewModel>
{
    private readonly DevFreelaDbContext _dbContext;
    private readonly IProjectRepository _repository;
    
    public DeleteProjectHandler(DevFreelaDbContext dbContext, IProjectRepository repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    public async Task<ResultViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetById(request.Id, cancellationToken);
        if (project is null)
            return ResultViewModel.Error("Project not found");
        project.SetAsDeleted();
        await _repository.Update(project, cancellationToken);
        return ResultViewModel.Success();
    }
}