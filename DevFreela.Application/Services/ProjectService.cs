using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services;

public class ProjectService : IProjectService
{
    private readonly DevFreelaDbContext _dbContext;
    
    public ProjectService(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public ResultViewModel<List<ProjectItemViewModel>> GetAll(string search = "", int page = 1, int size = 3)
    {
        var projects = _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
            .Skip(page * size)
            .Take(size)
            .ToList();
        
        var projectViewModels = projects
            .Select(ProjectItemViewModel.FromEntity)
            .ToList();
        
        return ResultViewModel<List<ProjectItemViewModel>>.Success(projectViewModels);
    }

    public ResultViewModel<ProjectViewModel> GetById(int id)
    {
        var project = _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Include(p => p.Comments)
            .SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return ResultViewModel<ProjectViewModel>.Error("Project not found");
        
        var projectViewModel = ProjectViewModel.FromEntity(project);
        
        return ResultViewModel<ProjectViewModel>.Success(projectViewModel);
    }

    public ResultViewModel<int> Insert(CreateProjectInputModel inputModel)
    {
        var project = inputModel.ToEntity();
        
        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();
        
        return ResultViewModel<int>.Success(project.Id);
    }

    public ResultViewModel Update(UpdateProjectInputModel inputModel)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == inputModel.IdProject);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
        
        _dbContext.Projects.Update(project);
        
        _dbContext.SaveChanges();
        
        return ResultViewModel.Success();
    }

    public ResultViewModel Delete(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.SetAsDeleted();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return ResultViewModel.Success();
    }

    public ResultViewModel Start(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.Start();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return ResultViewModel.Success();
    }

    public ResultViewModel Complete(int id)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        project.Complete();
        _dbContext.Projects.Update(project);
        _dbContext.SaveChanges();
        
        return ResultViewModel.Success();
    }

    public ResultViewModel InsertComment(int id, CreateProjectCommentInputModel inputModel)
    {
        var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
        
        if (project is null)
            return ResultViewModel.Error("Project not found");
        
        var comment = new ProjectComment(inputModel.Content, inputModel.IdProject, inputModel.IdUser);
        
        project.Comments.Add(comment);
        _dbContext.SaveChanges();
        return ResultViewModel.Success();
    }
}