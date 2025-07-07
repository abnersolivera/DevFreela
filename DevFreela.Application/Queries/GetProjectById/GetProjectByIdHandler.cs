using DevFreela.Application.Models;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ResultViewModel<ProjectViewModel>>
{
    private readonly IProjectRepository _repository;
    
    public GetProjectByIdHandler(IProjectRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResultViewModel<ProjectViewModel>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetDetailsById(request.Id, cancellationToken);
        
        if (project is null)
            return ResultViewModel<ProjectViewModel>.Error("Project not found");
        
        var projectViewModel = ProjectViewModel.FromEntity(project);
        
        return ResultViewModel<ProjectViewModel>.Success(projectViewModel);
    }
}