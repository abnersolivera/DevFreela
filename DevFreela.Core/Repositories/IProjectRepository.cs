using DevFreela.Core.Entities;

namespace DevFreela.Core.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAll(string search, int page, int rows, CancellationToken cancellationToken);
    Task<Project?> GetById(int id, CancellationToken cancellationToken);
    Task<Project?> GetDetailsById(int id, CancellationToken cancellationToken);
    Task<bool> Exists(int id, CancellationToken cancellationToken);
    Task<int> Add(Project project, CancellationToken cancellationToken);
    Task AddComment(ProjectComment comment, CancellationToken cancellationToken);
    Task Update(Project project, CancellationToken cancellationToken);
}