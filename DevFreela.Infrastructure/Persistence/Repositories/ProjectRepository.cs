using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DevFreelaDbContext _dbContext;
    
    public ProjectRepository(DevFreelaDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Project>> GetAll(string search, int page, int rows, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
            .Skip(page * rows)
            .Take(rows)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetById(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Project?> GetDetailsById(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .Include(p => p.Comments)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Projects.AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<int> Add(Project project, CancellationToken cancellationToken)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return project.Id;
    }

    public async Task AddComment(ProjectComment comment, CancellationToken cancellationToken)
    {
        await _dbContext.ProjectComments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(Project project, CancellationToken cancellationToken)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}