using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.InsertComment;

public class InsertCommentHandler : IRequestHandler<InsertCommentCommand, ResultViewModel>
{
    private readonly DevFreelaDbContext _dbContext;
    
    public InsertCommentHandler(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ResultViewModel> Handle(InsertCommentCommand request, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Id == request.IdProject, cancellationToken);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
        
        _dbContext.ProjectComments.Add(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ResultViewModel.Success();
    }
}