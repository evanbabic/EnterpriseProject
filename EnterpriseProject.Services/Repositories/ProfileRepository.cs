using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class ProfileRepository : IProfileServices
    {
        private readonly AppDbContext _dbContext;

        public ProfileRepository(AppDbContext dbContext) { _dbContext = dbContext; }


        public Profile getProfile(int userId)
        {
            return _dbContext.Profiles
                .Include(p => p.User)
                .ThenInclude(pr => pr.Projects)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.UserId == userId);
        }

        public void editProfile(Profile updatedProfile)
        {
            // getting the existing profile based on the userId from the database
            var existingProfile = _dbContext.Profiles
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == updatedProfile.UserId);

            if (existingProfile != null)
            {
                // Update the AboutMe field
                existingProfile.AboutMe = updatedProfile.AboutMe;

                // Update the ProfilePicturePath (if new profile picture is provided)
                if (!string.IsNullOrEmpty(updatedProfile.ProfilePicturePath))
                {
                    existingProfile.ProfilePicturePath = updatedProfile.ProfilePicturePath;
                }

                // Update the BannerImagePath (if new banner image is provided)
                if (!string.IsNullOrEmpty(updatedProfile.BannerImagePath))
                {
                    existingProfile.BannerImagePath = updatedProfile.BannerImagePath;
                }

                // Save changes to the database
                _dbContext.Profiles.Update(existingProfile);
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the profile doesn't exist
                throw new KeyNotFoundException("Profile not found for the provided user ID.");
            }
        }

    }
}
