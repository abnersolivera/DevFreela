using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CompleteProject;

public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
{
    private readonly IProjectRepository _repository;
    
    public CompleteProjectHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    
    public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetById(request.Id, cancellationToken);
        if (project is null)
            return ResultViewModel.Error("Project not found");
        project.Complete();
        await _repository.Update(project, cancellationToken);
        return ResultViewModel.Success();
    }
}