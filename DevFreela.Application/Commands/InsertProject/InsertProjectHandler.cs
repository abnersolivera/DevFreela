using DevFreela.Application.Models;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject;

public class InsertProjectHandler : IRequestHandler<InsertProjectCommand, ResultViewModel<int>>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public InsertProjectHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ResultViewModel<int>> Handle(InsertProjectCommand request, CancellationToken cancellationToken)
    {
        var project = request.ToEntity();
        
        await _dbContext.Projects.AddAsync(project, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResultViewModel<int>.Success(project.Id);
    }
}