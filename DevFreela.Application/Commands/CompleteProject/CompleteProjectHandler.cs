using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CompleteProject;

public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public CompleteProjectHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.Complete();
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResultViewModel.Success();
    }
}