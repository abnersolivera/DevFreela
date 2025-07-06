using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ResultViewModel>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public UpdateProjectHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ResultViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Id == request.IdProject, cancellationToken);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.Update(request.Title, request.Description, request.TotalCost);
        
        _dbContext.Projects.Update(project);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResultViewModel.Success();
    }
}