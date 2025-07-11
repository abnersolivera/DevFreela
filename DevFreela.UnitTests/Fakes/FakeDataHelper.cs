using Bogus;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;

namespace DevFreela.UnitTests.Fakes;

public class FakeDataHelper
{
    private static readonly Faker _faker = new Faker("pt_BR");

    public static Project CreateFakeProjectV1()
    {
        return new(_faker.Commerce.ProductName(), _faker.Lorem.Sentence(), _faker.Random.Number(10, 20),
            _faker.Random.Number(10, 20), _faker.Random.Decimal(1000, 10000));
    }

    private static readonly Faker<Project> _projectFaker = new Faker<Project>()
        .CustomInstantiator(f => new Project(
            f.Commerce.ProductName(),
            f.Lorem.Sentence(),
            f.Random.Number(1, 20),
            f.Random.Number(1, 20),
            f.Random.Decimal(1000, 10000)
        ));
    
    private static readonly Faker<InsertProjectCommand> _insertProjectCommandFaker = new Faker<InsertProjectCommand>()
        .RuleFor(c => c.Title, f => f.Commerce.ProductName())
        .RuleFor(c => c.Description, f => f.Lorem.Sentence())
        .RuleFor(c => c.IdClient, f => f.Random.Number(10, 20))
        .RuleFor(c => c.IdFreelancer, f => f.Random.Number(10, 20))
        .RuleFor(c => c.TotalCost, f => f.Random.Decimal(1000, 10000));

    public static Project CreateFakeProject() => _projectFaker.Generate();
    public static IEnumerable<Project> CreateFakeProjects() => _projectFaker.Generate(5);
    public static InsertProjectCommand CreateFakeInsertProjectCommand() => _insertProjectCommandFaker.Generate();
    public static DeleteProjectCommand CreateFakeDeleteProjectCommand(int id) => new (id);
}