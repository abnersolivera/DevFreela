using DevFreela.Core.Entities;

namespace DevFreela.API.Models;

public class ProjectViewModel
{
    public ProjectViewModel(int id, string title, string description, decimal totalCost, int idClient, int idFreelancer, string clientName, string freelancerName, List<ProjectComment> comments)
    {
        Id = id;
        Title = title;
        Description = description;
        TotalCost = totalCost;
        IdClient = idClient;
        IdFreelancer = idFreelancer;
        ClientName = clientName;
        FreelancerName = freelancerName;
        Comments = comments.Select(c => c.Content).ToList();
    }
    
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal TotalCost { get; private set; }
    public int IdClient { get; private set; }
    public int IdFreelancer { get; private set; }
    public string ClientName { get; private set; }
    public string FreelancerName { get; private set; }
    public List<string> Comments { get; private set; }
    
    public static ProjectViewModel FromEntity(Project entity)
    {
        return new ProjectViewModel(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.TotalCost,
            entity.IdClient,
            entity.IdFreelancer,
            entity.Client.FullName,
            entity.Freelancer.FullName,
            entity.Comments);
    }
    
}