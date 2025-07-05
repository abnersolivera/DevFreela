using DevFreela.Application.Models;

namespace DevFreela.Application.Services;

public interface IProjectService
{
    ResultViewModel<List<ProjectItemViewModel>> GetAll(string search = "", int page = 1, int size = 3);
    ResultViewModel<ProjectViewModel> GetById(int id);
    ResultViewModel<int> Insert(CreateProjectInputModel inputModel);
    ResultViewModel Update(UpdateProjectInputModel inputModel);
    ResultViewModel Delete(int id);
    ResultViewModel Start(int id);
    ResultViewModel Complete(int id);
    ResultViewModel InsertComment(int id, CreateProjectCommentInputModel inputModel);
    
}