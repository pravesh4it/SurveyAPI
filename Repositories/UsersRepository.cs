using ABC.Data;
using ABC.Models.Domain;
using ABC.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace ABC.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AbcDbContext dbContext;
        private readonly DataManager _dataManager;

        public UsersRepository(AbcDbContext dbContext, DataManager dataManager)
        {
            this.dbContext = dbContext;
            _dataManager = dataManager;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<UserDto> users = new List<UserDto>();
            try
            {
                var objDictionary = new Dictionary<string, string>();
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_allusers]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    users = dt.ToList<UserDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return users;

        }

        public async Task<UserInfo> GetUserByIdAsync(string userId)
        {
            return await dbContext.UserInfoes
                                 .Include(u => u.Department)
                                 .FirstOrDefaultAsync(u => u.AspNetUsersId == userId);
        }

        public async Task<UserInfo> AddUserAsync(RegisterRequestDto registerRequestDto, string AspNetUserId)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Id = new Guid();
            userInfo.FirstName = registerRequestDto.FirstName;
            userInfo.LastName = registerRequestDto.LastName;
            userInfo.DesignationId = registerRequestDto.DesignationId;
            userInfo.AspNetUsersId = AspNetUserId;
            userInfo.ContactNo = registerRequestDto.ContactNo;
            userInfo.ImageUrl = "fakeimg";
            userInfo.UserTypeId = "Admin";
            userInfo.IsFirstLogin = true;
            userInfo.CreatedDate = DateTime.Now;
            userInfo.LastModifiedDate = DateTime.Now;
            userInfo.LastModifiedBy = AspNetUserId;
            userInfo.DepartmentId = Guid.Parse("7389B2D3-CBD2-41CC-95BC-C2707DA1F6E0");
            dbContext.UserInfoes.Add(userInfo);
            await dbContext.SaveChangesAsync();
            return userInfo;
        }

        public async Task<UserInfo> UpdateUserAsync(UserInfo user)
        {
            dbContext.UserInfoes.Update(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await dbContext.UserInfoes.FindAsync(userId);
            if (user == null) return false;

            dbContext.UserInfoes.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserOptionsDto> Getoptions()
        {
            List<OptionsDto> optionsDtos = new List<OptionsDto>();
            UserOptionsDto userOptionsDto = new UserOptionsDto();


            // Fetch all records from the Designation table
            var designations = await dbContext.Designation.ToListAsync();

            // Map the results to OptionsDto
            userOptionsDto.Designations = optionsDtos = designations.Select(d => new OptionsDto
            {
                Id = d.Id.ToString(), // Assuming Designation has an Id field
                Name = d.Name // Assuming Designation has a Name field
            }).ToList();

            // Fetch all records from the Designation table
            var departments = await dbContext.Departments.ToListAsync();

            // Map the results to OptionsDto
            userOptionsDto.Departments = optionsDtos = departments.Select(d => new OptionsDto
            {
                Id = d.Id.ToString(), // Assuming Designation has an Id field
                Name = d.Name // Assuming Designation has a Name field
            }).ToList();

            var roles = await dbContext.Roles.ToListAsync();
            // Add other types handling logic if necessary
            // Map the results to OptionsDto
            userOptionsDto.Roles = optionsDtos = roles.Select(d => new OptionsDto
            {
                Id = d.Id.ToString(), // Assuming Designation has an Id field
                Name = d.Name // Assuming Designation has a Name field
            }).ToList();

            return userOptionsDto;
        }

        public async Task<string> AddEmailRegisterAsync(string Email, string AspNetUserId)
        {
            MailQueue mailQueue = new MailQueue();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@Email", Email },
                    { "@AspNetUserId", AspNetUserId }
                };

                DataTable dt = await _dataManager.GetDataTableAsync("[usp_RegisterSendEmail]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    mailQueue = dt.ToSingle<MailQueue>();
                }
            }
            catch (Exception ex)
            {

            }
            return "success";
        }

        public async Task<MailQueue> AddEmailRegisterUserAsync(string Email, string AspNetUserId)
        {
            MailQueue mailQueue = new MailQueue();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@Email", Email },
                    { "@AspNetUserId", AspNetUserId }
                };

                DataTable dt = await _dataManager.GetDataTableAsync("[usp_RegisterSendEmailUser]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    mailQueue = dt.ToSingle<MailQueue>();
                }
            }
            catch (Exception ex)
            {

            }
            return mailQueue;
        }

        public async Task<UserProfile> AddUserProfileAsync(RegisterUserDto dto, string AspNetUserId)
        {
            try
            {
                var profile = new UserProfile
                {
                    Id = new Guid(),
                    UserId = AspNetUserId,
                    FullName = dto.PersonalInfo.Name,
                    Dob = dto.PersonalInfo.Dob,
                    Gender = dto.PersonalInfo.Gender,
                    Country = dto.PersonalInfo.Country,
                    Zip = dto.PersonalInfo.Zip,
                    Income = dto.PersonalInfo.Income,

                    MaritalStatus = dto.Demographics.MaritalStatus,
                    Children = dto.Demographics.Children,
                    HouseholdSize = dto.Demographics.HouseholdSize,
                    EducationLevel = dto.Demographics.EducationLevel,
                    EmploymentStatus = dto.Demographics.EmploymentStatus,

                    Devices = string.Join(",", dto.Technology.Devices),
                    ShoppingFrequency = dto.Technology.ShoppingFrequency,

                    DomesticTravel = dto.Travel.Domestic,
                    InternationalTravel = dto.Travel.International,

                    MediaPlatforms = string.Join(",", dto.Media.Platforms),

                    HealthStatus = dto.Health.Status,
                    Chronic = dto.Health.Chronic,
                    SmokingAlcohol = dto.Health.SmokingAlcohol,
                    Exercise = dto.Health.Exercise,
                    Diet = dto.Health.Diet,

                    SurveyLength = dto.Survey.Length,
                    SurveyFormats = string.Join(",", dto.Survey.Formats),
                    SurveyFrequency = dto.Survey.Frequency,

                    RewardType = dto.Incentives.Reward,
                    RewardFrequency = dto.Incentives.Frequency,

                    ReferralSource = dto.Additional.Referral,

                    AcceptTerms = dto.Consent.Terms,
                    AcceptPrivacy = dto.Consent.PrivacyPolicy,
                    AcceptDataProcessing = dto.Consent.DataProcessing,
                    SubscribeNewsletter = dto.Consent.Newsletter,
                    ProfileURL = "default.png",
                    CreatedOn = DateTime.Now
                };

                dbContext.UserProfiles.Add(profile);
                await dbContext.SaveChangesAsync();


                return profile;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public async Task<UserProfile> GetUserProfileAsync(string user_id)
        {
            return await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.UserId == user_id);
        }
        public async Task<UserAdminProfileDto?> GetAdminProfileAsync(string userId)
        {
            // 1) Load the basic profile (read-only)
            var u = await dbContext.UserInfoes
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.AspNetUsersId == userId);

            if (u == null) return null;

            // 2) Load roles via Identity join
            var roles = await (
                from ur in dbContext.UserRoles.AsNoTracking()
                join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                where ur.UserId == userId
                select new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                }
            ).ToListAsync();

            // 3) Map to DTO
            return new UserAdminProfileDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                DesignationName = u.Designation != null ? u.Designation.Name : null,
                DepartmentName = u.Department != null ? u.Department.Name : null,
                ContactNo = u.ContactNo,
                Roles = roles,
                Email= dbContext.Users.FirstOrDefault(s=>s.Id.ToString()==u.AspNetUsersId.ToString()).UserName
            };
        }

        
    }
}
