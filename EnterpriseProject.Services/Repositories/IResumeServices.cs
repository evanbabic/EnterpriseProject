using EnterpriseProject.Entities;

namespace EnterpriseProject.Services.Repositories;

public interface IResumeServices
{
	void AddResume(Resume employee);
	Resume? GetResume(int id);
	void UpdateResume(Resume employee);
	void DeleteResume(int id);

	Resume? GetResumeByUserId(int userId);

	IEnumerable<Resume> GetResumes();
}