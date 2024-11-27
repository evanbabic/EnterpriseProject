using EnterpriseProject.Entities;

namespace EnterpriseProject.Services.Repositories;

public interface IResumeRepository
{
	void AddResume(Resume employee);
	Resume? GetResume(int id);
	void UpdateResume(Resume employee);
	void DeleteResume(int id);

	IEnumerable<Resume> GetResumes();
}