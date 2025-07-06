using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetAllProjects;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, ResultViewModel<List<ProjectItemViewModel>>>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public GetAllProjectsHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ResultViewModel<List<ProjectItemViewModel>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (request.Search == "" || p.Title.Contains(request.Search) || p.Description.Contains(request.Search)))
            .Skip(request.Page * request.Rows)
            .Take(request.Rows)
            .ToListAsync(cancellationToken);
        
        var projectViewModels = projects
            .Select(ProjectItemViewModel.FromEntity)
            .ToList();
        
        return ResultViewModel<List<ProjectItemViewModel>>.Success(projectViewModels);
    }
}