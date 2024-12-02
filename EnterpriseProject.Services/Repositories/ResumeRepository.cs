using EnterpriseProject.Entities;
using System;

namespace EnterpriseProject.Services.Repositories;

public class ResumeRepository(AppDbContext appDbContext) : IResumeServices
{
	private readonly AppDbContext appDbContext = appDbContext;

	public void AddResume(Resume employee)
	{
		appDbContext.Resumes.Add(employee);
		appDbContext.SaveChanges();
	}

	public Resume? GetResume(int id)
	{
		Resume? resume = appDbContext.Resumes.Find(id);
		if (resume == null) return null;

		resume.User = appDbContext.Users.Find(resume.UserId)!;

		return resume;
	}

	public Resume? GetResumeByUserId(int userId)
	{
		var resume = appDbContext.Resumes.FirstOrDefault(r => r.UserId == userId);

		if (resume != null)
		{
			resume.User = appDbContext.Users.Find(userId);
		}

		return resume;

	}


	public void UpdateResume(Resume employee)
	{
		appDbContext.Resumes.Update(employee);
		appDbContext.SaveChanges();
	}

	public void DeleteResume(int id)
	{
		Resume? resume = appDbContext.Resumes.Find(id);
		if (resume == null) return;

		appDbContext.Resumes.Remove(resume);
		appDbContext.SaveChanges();
	}

	public IEnumerable<Resume> GetResumes()
	{
		IEnumerable<Resume> resumes = [.. appDbContext.Resumes];
		foreach (var resume in resumes) resume.User = appDbContext.Users.Find(resume.UserId)!;

		return resumes;
	}
}