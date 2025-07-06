using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultViewModel>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public DeleteProjectHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.SetAsDeleted();
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResultViewModel.Success();
    }
}