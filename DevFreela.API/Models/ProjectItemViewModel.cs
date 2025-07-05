using DevFreela.Core.Entities;

namespace DevFreela.API.Models;

public class ProjectItemViewModel
{
    public ProjectItemViewModel(int id, string title, decimal totalCost, string clientName, string freelancerName)
    {
        Id = id;
        Title = title;
        TotalCost = totalCost;
        ClientName = clientName;
        FreelancerName = freelancerName;
    }
    
    public int Id { get; private set; }
    public string Title { get; private set; }
    public decimal TotalCost { get; private set; }
    public string ClientName { get; private set; }
    public string FreelancerName { get; private set; }
    
    public static ProjectItemViewModel FromEntity(Project entity)
    {
        return new ProjectItemViewModel(
            entity.Id,
            entity.Title,
            entity.TotalCost,
            entity.Client.FullName,
            entity.Freelancer.FullName);
    }
}