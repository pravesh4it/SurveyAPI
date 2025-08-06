
using ABC.Data;
using ABC.Migrations;
using ABC.Models.Domain;
using ABC.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace ABC.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly AbcDbContext dbContext;
        private readonly DataManager _dataManager;
        private readonly ClientSetting clientSetting;
        public SurveyRepository(AbcDbContext dbContext, DataManager dataManager, IOptions<ClientSetting> clientSettingOptions)
        {
            this.dbContext = dbContext;
            this._dataManager = dataManager;
            this.clientSetting = clientSettingOptions.Value;
        }
        public async Task<object> GetDataOptionsAsync()
        {
            //var salesManagers = await dbContext.salesManagers
            //    .Select(sm => new { id = sm.Id, name = sm.UserId })
            //.ToListAsync();

            //var projectManagers = await dbContext.projectManagers
            //    .Select(pm => new { id = pm.Id, name = pm.UserId })
            //.ToListAsync();
            // For Project Managers

            var projectManagers = await dbContext.UserInfoes
                .Where(ui => ui.DesignationId == dbContext.Designation
                    .Where(d => d.Name == "Project Manager")
                    .Select(d => d.Id)
                    .FirstOrDefault())
                .Select(ui => new
                {
                    id = ui.AspNetUsersId,
                    name = ui.FirstName + " " + ui.LastName
                })
                .ToListAsync();

            // For Sales Managers
            var salesManagers = await dbContext.UserInfoes
                .Where(ui => ui.DesignationId == dbContext.Designation
                    .Where(d => d.Name == "Sales Manager")
                    .Select(d => d.Id)
                    .FirstOrDefault())
                .Select(ui => new
                {
                    id = ui.AspNetUsersId,
                    name = ui.FirstName + " " + ui.LastName
                })
                .ToListAsync();

            var countries = await dbContext.Countries
                .Select(c => new { id = c.Id, name = c.Name })
            .ToListAsync();

            var languages = await dbContext.MultiSelects
                .Where(ms => ms.SelectionType == "Language")
                .Select(ms => new { id = ms.Id, name = ms.Name })
            .ToListAsync();

            var clients = await dbContext.Clients
                .Where(c => c.ClientType.Id.ToString() == clientSetting.Client)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToListAsync();


            var status = await dbContext.MultiSelects
                .Where(ms => ms.SelectionType == "status")
                .Select(ms => new { id = ms.Id, name = ms.Name })
                .ToListAsync();

            var currency = await dbContext.currencies
                .Select(c => new { id = c.Id, name = c.Name, symbol = c.Symbol })
            .ToListAsync();

            var quiztype = await dbContext.MultiSelects
                .Where(ms => ms.SelectionType == "Quiz Type")
                .Select(ms => new { id = ms.Id, name = ms.Name })
                .ToListAsync();

            var surveys = await dbContext.Surveys
                .Select(c => new { id = c.Id, name = c.Name })
            .ToListAsync();

            return new
            {
                sales_managers = salesManagers,
                project_managers = projectManagers,
                countries = countries,
                languages = languages,
                clients = clients,
                status = status,
                currencies = currency,
                quiztype = quiztype,
                surveys = surveys
            };

        }
        public async Task<SurveyViewDto> GetSurveyClientPartnersAsync(Guid Id)
        {
            // get survey and client data
            // get partners data
            SurveyViewDto surveyViewDto = new SurveyViewDto();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@Id", Id.ToString() }
                };
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_survey_view]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    surveyViewDto = dt.ToSingle<SurveyViewDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return surveyViewDto;
        }
        public async Task<List<SurveyDto>> GetSurveyListAsync()
        {
            List<SurveyDto> surveys = new List<SurveyDto>();
            try
            {
                var objDictionary = new Dictionary<string, string>();
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_survey_all]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    surveys = dt.ToList<SurveyDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return surveys;

        }
        public async Task<List<PartnerSurveyDto>> GetSurveyPartnersListAsync(Guid Id)
        {
            List<PartnerSurveyDto> partnerssurvey = new List<PartnerSurveyDto>();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@survey_id", Id.ToString() }
                };
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_survey_partners]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    partnerssurvey = dt.ToList<PartnerSurveyDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return partnerssurvey;

        }
        public async Task<string> CreateAsync(SurveyAddDto surveyDto)
        {
            try
            {
                // Convert string UUIDs to Guid
                var projectManagerIds = surveyDto.ProjectManagers.Select(Guid.Parse).ToList();
                var salesManagerIds = surveyDto.SalesManagers.Select(Guid.Parse).ToList();

                List<ProjectManager> projectManagers = projectManagerIds
                    .Select(id => new ProjectManager
                    {
                        UserId = id.ToString(),
                        //SurveyId = survey.Id // Assuming survey.Id is available
                    }).ToList();

                // Create SalesManager objects from the salesManagerIds list
                List<SalesManager> salesManagers = salesManagerIds
                    .Select(id => new SalesManager
                    {
                        UserId = id.ToString(),
                        //SurveyId = survey.Id // Assuming survey.Id is available
                    }).ToList();


                var survey = new Survey
                {
                    Id = Guid.NewGuid(),
                    Title = surveyDto.Title,
                    Name = "PDR" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString(),
                    Status = dbContext.MultiSelects
                        .Where(m => m.SelectionType == "status" && m.Name == "open")
                        .Select(m => m.Id)
                        .FirstOrDefault().ToString(),
                    Language = surveyDto.Language,
                    Country = surveyDto.Country,
                    FilledTimeInDays = surveyDto.FilledTime,
                    Completes = surveyDto.Completes,
                    LengthOfSurveyInMinutes = surveyDto.LengthOfSurvey,
                    Incidence = surveyDto.Incidence,
                    ClientOrPartner = surveyDto.Client,
                    ClientLink = surveyDto.ClientLink,
                    ProjectManagers = projectManagers,
                    SalesManagers = salesManagers,
                    CreatedById = surveyDto.CreatedById,
                    Success_Link = "",
                    Disqualification_Link = "",
                    QuotaFull_Link = "",
                    LaunchedDate = DateOnly.FromDateTime(DateTime.Now),
                    //EndDate = surveyDto.EndDate,
                    CurrencyId = surveyDto.Currency,
                    ClientIR = surveyDto.ClientIR,
                    SurveyQuota = surveyDto.SurveyQuota,
                    ClientRate = surveyDto.ClientRate,
                    IsDeleted = false,
                    PreScreener = surveyDto.PreScreener
                };

                dbContext.Surveys.Add(survey);
                await dbContext.SaveChangesAsync();
                string survey_id = survey.Id.ToString();

                // Add Default Partner Dynamic Research

                var Partner = new PartnerSurvey
                {
                    Id = Guid.NewGuid(),
                    PartnerId = Guid.Parse(surveyDto.DefaultPartner),
                    Rate = 0,
                    AddedBy = surveyDto.CreatedById,
                    AddedOn = DateTime.UtcNow,
                    Quota = surveyDto.SurveyQuota,
                    SurveyUuid = Guid.Parse(survey_id), // link to new survey
                };
                dbContext.partnerSurveys.Add(Partner);
                await dbContext.SaveChangesAsync();

                return survey_id;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public async Task<bool> DeleteAsync(Guid surveyId)
        {
            try
            {
                // Fetch the survey including related ProjectManagers and SalesManagers
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == surveyId);

                if (survey == null)
                {
                    return false;
                }
                survey.IsDeleted = true;
                await dbContext.SaveChangesAsync(); // Save before adding new ones
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }
        Task<Client> ISurveyRepository.GetClentAsync(Guid clientId)
        {
            throw new NotImplementedException();
        }
        public async Task<string> UpdateAsync(Guid surveyId, SurveyAddDto surveyDto)
        {
            try
            {
                // Fetch the survey including related ProjectManagers and SalesManagers
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == surveyId);

                if (survey == null)
                {
                    return "Survey not found";
                }

                // Update survey fields
                survey.Title = surveyDto.Title;
                survey.Language = surveyDto.Language;
                survey.Country = surveyDto.Country;
                survey.FilledTimeInDays = surveyDto.FilledTime;
                survey.Completes = surveyDto.Completes;
                survey.LengthOfSurveyInMinutes = surveyDto.LengthOfSurvey;
                survey.Incidence = surveyDto.Incidence;
                survey.ClientOrPartner = surveyDto.Client;
                survey.ClientLink = surveyDto.ClientLink;
                //survey.LaunchedDate = surveyDto.LaunchedDate;
                //survey.EndDate = surveyDto.EndDate;
                survey.CurrencyId = surveyDto.Currency;
                survey.ClientIR = surveyDto.ClientIR;
                survey.SurveyQuota = surveyDto.SurveyQuota;
                survey.ClientRate = surveyDto.ClientRate;
                survey.PreScreener = surveyDto.PreScreener;

                // Convert string UUIDs to Guid
                var projectManagerIds = surveyDto.ProjectManagers.Select(Guid.Parse).ToList();
                var salesManagerIds = surveyDto.SalesManagers.Select(Guid.Parse).ToList();

                // Remove existing ProjectManagers
                var existingProjectManagers = dbContext.projectManagers.Where(pm => pm.SurveyId == surveyId);
                dbContext.projectManagers.RemoveRange(existingProjectManagers);

                // Remove existing SalesManagers
                var existingSalesManagers = dbContext.salesManagers.Where(sm => sm.SurveyId == surveyId);
                dbContext.salesManagers.RemoveRange(existingSalesManagers);

                await dbContext.SaveChangesAsync(); // Save before adding new ones

                // Now add updated ProjectManagers
                foreach (var id in projectManagerIds)
                {
                    survey.ProjectManagers.Add(new ProjectManager { UserId = id.ToString(), SurveyId = survey.Id });
                }

                // Add updated SalesManagers
                foreach (var id in salesManagerIds)
                {
                    survey.SalesManagers.Add(new SalesManager { UserId = id.ToString(), SurveyId = survey.Id });
                }

                await dbContext.SaveChangesAsync(); // Save changes after re-adding


                // Save changes to the database
                await dbContext.SaveChangesAsync();
                return survey.Id.ToString();
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return ex.Message;
            }
        }

        public async Task<string> UpdateStatusAsync(Guid surveyId, SurveyStatusUpdateDto surveyStatusUpdateDto)
        {
            try
            {
                // Fetch the survey including related ProjectManagers and SalesManagers
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == surveyId);
                if (survey == null)
                {
                    return "Survey not found";
                }
                // Update survey fields
                survey.Status = surveyStatusUpdateDto.StatusId;
                await dbContext.SaveChangesAsync(); // Save before adding new ones
                return survey.Id.ToString();
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return ex.Message;
            }
        }

        public async Task<string> SurveyAddPartnerAsync(SurveyAddPartnerDto surveyAddPartnerDto)
        {
            try
            {
                PartnerSurvey partnerSurvey = new PartnerSurvey();
                partnerSurvey.Id = new Guid();
                partnerSurvey.PartnerId = surveyAddPartnerDto.PartnerId;
                partnerSurvey.AvailableVariable = surveyAddPartnerDto.AvailableVariable;
                partnerSurvey.Rate = surveyAddPartnerDto.Rate;
                partnerSurvey.AddedBy = surveyAddPartnerDto.AddedBy;
                partnerSurvey.AddedOn = DateTime.UtcNow;
                partnerSurvey.Quota = surveyAddPartnerDto.Quota;
                partnerSurvey.SurveyUuid = surveyAddPartnerDto.SurveyUuid;

                partnerSurvey.PartnerQuotaLink = surveyAddPartnerDto.PartnerQuotaLink;
                partnerSurvey.PartnerSuccessLink = surveyAddPartnerDto.PartnerSuccessLink;
                partnerSurvey.PartnerDisqualificationLink = surveyAddPartnerDto.PartnerDisqualificationLink;
                partnerSurvey.SecurityFailLink = surveyAddPartnerDto.SecurityFailLink;
                partnerSurvey.PausedLink = surveyAddPartnerDto.PausedLink;


                await dbContext.AddAsync(partnerSurvey);
                await dbContext.SaveChangesAsync();



                return partnerSurvey.Id.ToString();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<object> SurveyAddResponseAsync(SurveyResponseDto surveyResponseDto)
        {
            try
            {
                // Fetch SurveyId and PartnerId from PartnerSurveys table
                var partnerSurvey = await dbContext.partnerSurveys
                    .FirstOrDefaultAsync(ps => ps.Id == surveyResponseDto.SurveyPartnerId);

                if (partnerSurvey == null)
                {
                    throw new Exception($"No entry found in PartnerSurveys with Id {surveyResponseDto.SurveyPartnerId}");
                }

                Guid survey_id = partnerSurvey.SurveyUuid;

                // ✅ Check if a response already exists for this SurveyId and RespondentId
                var existingResponse = await dbContext.surveyResponses
                    .FirstOrDefaultAsync(sr => sr.SurveyId == survey_id && sr.RespondentId == surveyResponseDto.RespondentId);

                if (existingResponse != null)
                {
                    return new
                    {
                        status = "already exists",
                        response_uuid = existingResponse.Id,
                        response_link = existingResponse.ClientURL
                    };
                }

                var Survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == survey_id);

                string survey_link = Survey.ClientLink;

                // Create the SurveyResponse object
                SurveyResponse surveyResponse = new SurveyResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    AddedBy = surveyResponseDto.addedby,
                    SurveyId = partnerSurvey.SurveyUuid,
                    PartnerId = partnerSurvey.PartnerId.ToString(),
                    RespondentId = surveyResponseDto.RespondentId,
                    RespondentIP = surveyResponseDto.RespondentIP,
                    Answers = "",
                    CreatedAt = DateTime.UtcNow,
                    Status = "incomplete"
                };

                // Replace placeholder in client link
                string updatedLink = Regex.Replace(survey_link, @"[\{<][^}>]+[\}>]", surveyResponseDto.RespondentId.ToString());
                surveyResponse.ClientURL = updatedLink;

                // Save to database
                await dbContext.AddAsync(surveyResponse);
                await dbContext.SaveChangesAsync();

                return new
                {
                    status = "created",
                    response_uuid = surveyResponse.Id.ToString(),
                    response_link = updatedLink
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SurveyAddResponseAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<SurveyResponseResult> SurveyCompleteResponseAsync(SurveyCompleteResponseDto surveyCompleteResponseDto)
        {
            try
            {
                try
                {
                    var uid = surveyCompleteResponseDto.uid.ToString();
                    var sid = surveyCompleteResponseDto.sid; // assuming sid is of compatible type

                    // Fetch SurveyResponse entry based on uid and sid
                    var surveyResponse = await dbContext.surveyResponses
                        .FirstOrDefaultAsync(sr => sr.RespondentId.ToLower() == uid.ToLower() && sr.SurveyId.ToString().ToLower() == sid.ToLower());

                    if (surveyResponse == null)
                    {
                        // Create new SurveyResponse entry if not found
                        //surveyResponse = new SurveyResponse
                        //{
                        //    Id = uid,
                        //    SurveyId = sid,
                        //    CreatedAt = DateTime.UtcNow
                        //};

                        //dbContext.surveyResponses.Add(surveyResponse);

                        return new SurveyResponseResult
                        {
                            Status = "Error",
                            Message = "Invalid"
                        };

                    }
                    if (surveyResponse.Status == "incomplete")
                    {
                        // Determine response type and update the database record accordingly
                        if (surveyCompleteResponseDto.response_type == "success")
                        {
                            surveyResponse.Status = "success";
                        }
                        else if (surveyCompleteResponseDto.response_type == "disqualify")
                        {
                            surveyResponse.Status = "disqualify";
                        }
                        else if (surveyCompleteResponseDto.response_type == "quotafull")
                        {
                            surveyResponse.Status = "quotafull";
                        }

                        surveyResponse.AddedBy = surveyCompleteResponseDto.addedby;
                        // Save changes to the database
                        await dbContext.SaveChangesAsync();
                        return new SurveyResponseResult
                        {
                            Status = "Success",
                            Message = "Survey Completed"
                        };
                    }
                    else
                    {
                        return new SurveyResponseResult
                        {
                            Status = "Error",
                            Message = "Survey Already Completed"
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error in SurveyCompleteResponseAsync: {ex.Message}");
                    throw;
                }
                //return new { message = "completed" };
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in SurveyCompleteResponseAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<object> GetSurveyReportAsync(string surveyId)
        {
            List<SurveyResposeReportDto> reportsurvey = new List<SurveyResposeReportDto>();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@survey_id", surveyId.ToString() }
                };
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_survey_report]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    reportsurvey = dt.ToList<SurveyResposeReportDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return reportsurvey;
        }
        public async Task<object> GetSurveyPreScreeningAsync(string surveyId)
        {
            List<SurveyPreScreenerDto> reportsurvey = new List<SurveyPreScreenerDto>();
            try
            {
                var objDictionary = new Dictionary<string, string>
                {
                    { "@survey_id", surveyId.ToString() }
                };
                DataTable dt = await _dataManager.GetDataTableAsync("[usp_survey_prescreener]", objDictionary);
                if (dt.Rows.Count > 0)
                {
                    reportsurvey = dt.ToList<SurveyPreScreenerDto>();
                }
            }
            catch (Exception ex)
            {

            }
            return reportsurvey;
        }
        public async Task<object> GetIsSurveyPreScreeningAsync(string surveyId)
        {
            bool isSurveyPreScreening = false;
            try
            {
                var partnerSurvey = await dbContext.partnerSurveys
                    .FirstOrDefaultAsync(ps => ps.Id.ToString() == surveyId);

                Guid survey_id = partnerSurvey.SurveyUuid;
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(sr => sr.Id == survey_id);

                if (survey != null && survey.PreScreener)
                {
                    bool hasPreScreeners = await dbContext.SurveyPreScreeners
                        .AnyAsync(ps => ps.SurveyId == survey_id.ToString());

                    isSurveyPreScreening = hasPreScreeners;
                }
            }
            catch (Exception ex)
            {
                // Optionally log exception
            }

            return isSurveyPreScreening;
        }

        public async Task<object> SurveyAddPreScreenerAsync(PreScreenerAddDto preScreenerAddDto)
        {
            try
            {

                string survey_id = preScreenerAddDto.SurveyId;

                // Create the SurveyResponse object
                SurveyPreScreener surveyPreScreener = new SurveyPreScreener
                {
                    Id = Guid.NewGuid(),
                    SurveyId = survey_id,
                    QuestionType = preScreenerAddDto.QuestionType,
                    Question = preScreenerAddDto.Question,
                    Option1 = preScreenerAddDto.Option1,
                    IsDelete = false,
                    AddedBy = preScreenerAddDto.AddedBy,
                    AddedOn = DateTime.Now
                };

                // Add to the database and save changes
                await dbContext.AddAsync(surveyPreScreener);
                await dbContext.SaveChangesAsync();
                //string updatedLink = Regex.Replace(survey_link, @"\{[^}]+\}", surveyResponse.Id.ToString());
                return new
                {
                    response_uuid = surveyPreScreener.Id.ToString(),
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<object> GetPartnersListAsync()
        {
            var clients = await dbContext.Clients
                .Select(c => new { id = c.Id, name = c.Name })
            .ToListAsync();

            return new
            {
                clients = clients
            };
        }
        public async Task<SurveyEditDto> GetSurveyByIdAsync(Guid surveyId)
        {
            try
            {
                var survey = await dbContext.Surveys
                    .Include(s => s.ProjectManagers)
                    .Include(s => s.SalesManagers)
                    .FirstOrDefaultAsync(s => s.Id == surveyId);

                if (survey == null)
                {
                    return null; // Survey not found
                }

                // Map entity to DTO
                var surveyDto = new SurveyEditDto
                {
                    Id = survey.Id.ToString(),
                    Title = survey.Title,
                    Name = survey.Name,
                    Status = survey.Status,
                    Language = survey.Language,
                    Country = survey.Country,
                    FilledTime = survey.FilledTimeInDays,
                    Completes = survey.Completes,
                    LengthOfSurvey = survey.LengthOfSurveyInMinutes,
                    Incidence = survey.Incidence,
                    Client = survey.ClientOrPartner,
                    ClientLink = survey.ClientLink,
                    LaunchedDate = survey.LaunchedDate,
                    EndDate = survey.EndDate,
                    Currency = survey.CurrencyId,
                    ClientIR = survey.ClientIR,
                    SurveyQuota = survey.SurveyQuota,
                    ClientRate = survey.ClientRate,
                    PreScreener = survey.PreScreener,
                    ProjectManagers = survey.ProjectManagers.Select(pm => pm.UserId.ToString()).ToList(),
                    SalesManagers = survey.SalesManagers.Select(sm => sm.UserId.ToString()).ToList()
                };

                return surveyDto;
            }
            catch (Exception ex)
            {
                // Log the error in production
                throw new Exception("Error fetching survey data: " + ex.Message);
            }
        }

        public async Task<string> CloneAsync(CloneSurveyDto cloneSurveyDto)
        {
            // get survey by id 
            var survey = await dbContext.Surveys
                   .Include(s => s.ProjectManagers)
                   .Include(s => s.SalesManagers)
                   .FirstOrDefaultAsync(s => s.Id.ToString() == cloneSurveyDto.Id);

            if (survey == null)
            {
                return null; // Survey not found
            }
            // create new //////

            try
            {

                var survey1 = new Survey
                {
                    Id = Guid.NewGuid(),
                    Title = survey.Title,
                    //Name = GenerateNextSurveyName(survey.Name),
                    Name = "PDR" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString(),
                    Status = dbContext.MultiSelects
                        .Where(m => m.SelectionType == "status" && m.Name == "open")
                        .Select(m => m.Id)
                        .FirstOrDefault().ToString(),
                    Language = survey.Language,
                    Country = survey.Country,
                    FilledTimeInDays = survey.FilledTimeInDays,
                    Completes = survey.Completes,
                    LengthOfSurveyInMinutes = survey.LengthOfSurveyInMinutes,
                    Incidence = survey.Incidence,
                    ClientOrPartner = survey.ClientOrPartner,
                    ClientLink = survey.ClientLink,
                    ProjectManagers = survey.ProjectManagers,
                    SalesManagers = survey.SalesManagers,
                    CreatedById = cloneSurveyDto.AddedBy,
                    Success_Link = "",
                    Disqualification_Link = "",
                    QuotaFull_Link = "",
                    LaunchedDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = null,
                    CurrencyId = survey.CurrencyId,
                    ClientIR = survey.ClientIR,
                    SurveyQuota = survey.SurveyQuota,
                    ClientRate = survey.ClientRate,
                    IsDeleted = false,
                    ParentId = survey.Id
                };


                dbContext.Surveys.Add(survey1);
                await dbContext.SaveChangesAsync();

                // 🔁 Clone PartnerSurvey records
                var partnerSurveys = await dbContext.Set<PartnerSurvey>()
                    .Where(p => p.SurveyUuid == survey.Id)
                    .ToListAsync();

                foreach (var partner in partnerSurveys)
                {
                    var clonedPartner = new PartnerSurvey
                    {
                        Id = Guid.NewGuid(),
                        PartnerId = partner.PartnerId,
                        AvailableVariable = partner.AvailableVariable,
                        Rate = partner.Rate,
                        AddedBy = cloneSurveyDto.AddedBy,
                        AddedOn = DateTime.UtcNow,
                        Quota = partner.Quota,
                        SurveyUuid = survey1.Id, // link to new survey

                        PartnerQuotaLink = ReplaceSurveyNameInUrl(partner.PartnerQuotaLink, survey.Name, survey1.Name),
                        PartnerSuccessLink = ReplaceSurveyNameInUrl(partner.PartnerSuccessLink, survey.Name, survey1.Name),
                        PartnerDisqualificationLink = ReplaceSurveyNameInUrl(partner.PartnerDisqualificationLink, survey.Name, survey1.Name),
                        SecurityFailLink = ReplaceSurveyNameInUrl(partner.SecurityFailLink, survey.Name, survey1.Name),
                        PausedLink = ReplaceSurveyNameInUrl(partner.PausedLink, survey.Name, survey1.Name)
                    };

                    dbContext.partnerSurveys.Add(clonedPartner);
                }
                await dbContext.SaveChangesAsync();
                string survey_id = survey1.Id.ToString();
                return survey_id;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string GenerateNextSurveyName(string baseName)
        {
            // Get existing surveys with the same base name pattern
            var existingNames = dbContext.Surveys
                .Where(s => s.Name.StartsWith(baseName))
                .Select(s => s.Name)
                .ToList();

            // Regular expression to extract numeric suffix
            Regex regex = new Regex($@"^{Regex.Escape(baseName)}(\d+)$");

            int maxSuffix = 0;

            foreach (var name in existingNames)
            {
                Match match = regex.Match(name);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int suffix))
                {
                    maxSuffix = Math.Max(maxSuffix, suffix);
                }
            }

            // Generate the next name with incremented suffix
            return $"{baseName}{(maxSuffix + 1):D2}"; // Ensures 2-digit format like "01", "02", etc.
        }

        public async Task<List<SurveyPreScreenerDto>> GetSurveyPreScreeningQuestAsync(string Id)
        {
            List<SurveyPreScreenerDto> PreScreener = new List<SurveyPreScreenerDto>();
            try
            {
                var partnerSurvey = await dbContext.partnerSurveys
                    .FirstOrDefaultAsync(ps => ps.Id.ToString() == Id);

                Guid survey_id = partnerSurvey.SurveyUuid;

                // Get the list of prescreener questions for the associated survey
                var questions = await dbContext.SurveyPreScreeners
                    .Where(q => q.SurveyId == survey_id.ToString())
                    .ToListAsync();

                // Map each question to the DTO
                PreScreener = questions.Select(q => new SurveyPreScreenerDto
                {
                    Id = q.Id.ToString(),
                    SurveyId = q.SurveyId,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Option1 = q.Option1,
                }).ToList();
            }
            catch (Exception ex)
            {
                // Optionally log exception
            }

            return PreScreener;
        }
        public async Task<bool> HasDisqualifyingAnswerAsync(List<ResponseDto> responses)
        {
            foreach (var response in responses)
            {
                var screener = await dbContext.SurveyPreScreeners
                    .FirstOrDefaultAsync(q => q.Id.ToString() == response.QuestionId && !q.IsDelete);

                if (screener == null) continue;

                // Extract [x] options
                var disqualifyingOptions = screener.Option1?
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(opt => opt.StartsWith("[x]"))
                    .Select(opt => opt.Replace("[x]", "").Trim())
                    .ToList();

                if (disqualifyingOptions == null || disqualifyingOptions.Count == 0)
                    continue;

                // Compare selected answer
                foreach (var disqualifyingOption in disqualifyingOptions)
                {
                    if (response.Answer?.IndexOf(disqualifyingOption, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }

            return false;
        }
        public string ReplaceSurveyNameInUrl(string url, string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            return url.Replace($"surveyName={oldName}", $"surveyName={newName}");
        }
        public async Task<bool> SurveyResponseUpdateStatusAsync(UpdateSurveyStatusDto surveystatusDto)
        {
            try
            {
                string survey_id = surveystatusDto.SurveyId;
                string id = surveystatusDto.Id;
                string status = surveystatusDto.Status;

                // get survey by id 
                var report1 = dbContext.surveyResponses.FirstOrDefault(r => r.Id.ToString().ToLower() == surveystatusDto.Id.ToLower());
                var report = await dbContext.surveyResponses
                    .FirstOrDefaultAsync(r =>
                        r.SurveyId.ToString().ToLower() == survey_id.ToLower() &&
                        r.Id.ToString().ToLower() == id.ToLower());

                if (report == null)
                    return false;

                report.Status = status;
                await dbContext.SaveChangesAsync();
                //string updatedLink = Regex.Replace(survey_link, @"\{[^}]+\}", surveyResponse.Id.ToString());
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in SurveyResponseUpdateStatusAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<string> AddRecontact(RecontactDto recontactDto)
        {
            if (recontactDto.file == null || recontactDto.file.Length == 0)
            {
                return "File is missing";
            }

            var entries = new List<SurveyResponse>();

            try
            {

                // step-1 Clone this survey
                // step-2 Read File and enter in the report table

                CloneSurveyDto cloneSurveyDto = new CloneSurveyDto();
                cloneSurveyDto.Id = recontactDto.SurveyId;
                cloneSurveyDto.AddedBy = recontactDto.CreatedById;
                var survey_id = await CloneAsync(cloneSurveyDto);

                using (var stream = new StreamReader(recontactDto.file.OpenReadStream()))
                {
                    string line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        try
                        {
                            var values = line.Split(',');
                            if (values.Length < 2) continue; // Skip if less than 2 columns

                            string survey_response_id = values[1].Trim();
                            string old_survey_id = recontactDto.SurveyId;

                            var report = await dbContext.surveyResponses
                                .FirstOrDefaultAsync(r =>
                                r.SurveyId.ToString().ToLower() == old_survey_id.ToLower() &&
                                r.Id.ToString().ToLower() == survey_response_id.ToLower());
                            // get object of response from survey_response

                            if (report != null)
                            {
                                entries.Add(new SurveyResponse
                                {
                                    ClientURL = values[0].Trim(),
                                    Id = values[1].Trim(),
                                    SurveyId = Guid.Parse(survey_id),
                                    PartnerId = report.PartnerId,
                                    CreatedAt = DateTime.UtcNow,
                                    RespondentId = report.RespondentId,
                                    ResponseDate = DateTime.UtcNow,
                                    Status = "incomplete"

                                });
                            }
                            dbContext.surveyResponses.AddRange(entries);
                            await dbContext.SaveChangesAsync();

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<SurveyEditDto> GetSurveyByPartnerIdAsync(Guid survey_partner_id)
        {
            try
            {
                // Step 1: Get the survey UUID using the partner survey ID
                var partnerSurvey = await dbContext.partnerSurveys
                    .FirstOrDefaultAsync(ps => ps.Id == survey_partner_id);

                if (partnerSurvey == null)
                {
                    return null; // Partner survey mapping not found
                }

                var surveyUuid = partnerSurvey.SurveyUuid;

                // Step 2: Fetch the actual survey using SurveyUuid
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == surveyUuid);

                if (survey == null)
                {
                    return null; // Survey not found
                }

                // Step 3: Map to DTO
                var surveyDto = new SurveyEditDto
                {
                    Id = survey.Id.ToString(),
                    Title = survey.Title,
                    Name = survey.Name,
                    Status = survey.Status,
                    Language = survey.Language,
                    Country = survey.Country,
                    FilledTime = survey.FilledTimeInDays,
                    Completes = survey.Completes,
                    LengthOfSurvey = survey.LengthOfSurveyInMinutes,
                    Incidence = survey.Incidence,
                    Client = survey.ClientOrPartner,
                    ClientLink = survey.ClientLink,
                    LaunchedDate = survey.LaunchedDate,
                    EndDate = survey.EndDate,
                    Currency = survey.CurrencyId,
                    ClientIR = survey.ClientIR,
                    SurveyQuota = survey.SurveyQuota,
                    ClientRate = survey.ClientRate,
                    PreScreener = survey.PreScreener
                };

                return surveyDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching survey data: " + ex.Message);
            }
        }

    }
}
