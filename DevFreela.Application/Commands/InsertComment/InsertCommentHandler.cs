using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.InsertComment;

public class InsertCommentHandler : IRequestHandler<InsertCommentCommand, ResultViewModel>
{
    private readonly IProjectRepository _repository;
    
    public InsertCommentHandler(IProjectRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResultViewModel> Handle(InsertCommentCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.Exists(request.IdProject, cancellationToken);
        if (!project)
            return ResultViewModel.Error("Project not found");
        var projectComment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
        await _repository.AddComment(projectComment, cancellationToken);
        return ResultViewModel.Success();
    }
}