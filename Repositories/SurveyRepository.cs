
using ABC.Data;
using ABC.Migrations;
using ABC.Models.Domain;
using ABC.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
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
        private readonly IWebHostEnvironment _environment;
        public SurveyRepository(AbcDbContext dbContext, DataManager dataManager, IOptions<ClientSetting> clientSettingOptions, IWebHostEnvironment environment)
        {
            this.dbContext = dbContext;
            this._dataManager = dataManager;
            this.clientSetting = clientSettingOptions.Value;
            _environment = environment;
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
                // Convert string UUIDs to Guid lists
                var projectManagerIds = surveyDto.ProjectManagers?.Select(Guid.Parse).ToList() ?? new List<Guid>();
                var salesManagerIds = surveyDto.SalesManagers?.Select(Guid.Parse).ToList() ?? new List<Guid>();

                // Build ProjectManager & SalesManager objects (UserId stored as string as you had)
                var projectManagers = projectManagerIds
                    .Select(id => new ProjectManager { UserId = id.ToString() })
                    .ToList();

                var salesManagers = salesManagerIds
                    .Select(id => new SalesManager { UserId = id.ToString() })
                    .ToList();

                // Create survey object
                var survey = new Survey
                {
                    Id = Guid.NewGuid(),
                    Title = surveyDto.Title,
                    Name = "P" + DateTime.Now.ToString("ddHHmmss") + DateTime.Now.Millisecond.ToString(),
                    Status = dbContext.MultiSelects
                        .Where(m => m.SelectionType == "status" && m.Name == "draft")
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
                    LaunchedDate = DateOnly.FromDateTime(DateTime.UtcNow), // or use surveyDto.LaunchDate if provided
                    CurrencyId = surveyDto.Currency,
                    ClientIR = surveyDto.ClientIR,
                    SurveyQuota = surveyDto.SurveyQuota,
                    ClientRate = surveyDto.ClientRate,
                    IsDeleted = false,
                    PreScreener = surveyDto.PreScreener,
                    UniqueLink = surveyDto.UniqueLink
                };

                // Start a transaction: create survey, partner survey, and add rate history entries atomically
                await using var tx = await dbContext.Database.BeginTransactionAsync();

                // 1) Add survey
                dbContext.Surveys.Add(survey);
                await dbContext.SaveChangesAsync();
                var survey_id = survey.Id.ToString();

                // 2) Add PartnerSurvey (default partner)
                var partnerIdGuid = Guid.Parse(surveyDto.DefaultPartner);
                var partnerSurvey = new PartnerSurvey
                {
                    Id = Guid.NewGuid(),
                    PartnerId = partnerIdGuid,
                    Rate = surveyDto.ClientRate,
                    AddedBy = surveyDto.CreatedById,
                    AddedOn = DateTime.UtcNow,
                    Quota = surveyDto.SurveyQuota,
                    SurveyUuid = survey.Id
                };
                dbContext.partnerSurveys.Add(partnerSurvey);
                await dbContext.SaveChangesAsync();

                // Determine the start date for the initial rates (use LaunchedDate if you want business date)
                DateTime startDate = DateTime.UtcNow;
              

                // 3) Close overlapping partner rates (if any) so we don't have overlapping active ranges
                var overlappingPartnerRates = await dbContext.RateHistory
                    .Where(r => r.EntityType == "Partner" && r.EntityId == partnerSurvey.Id
                                && r.StartDate < startDate
                                && (r.EndDate == null || r.EndDate >= startDate))
                    .ToListAsync();

                foreach (var prev in overlappingPartnerRates)
                {
                    prev.EndDate = DateTime.UtcNow;
                    dbContext.RateHistory.Update(prev);
                }

                // 4) Insert RateHistory for Survey
                var surveyRate = new RateHistory
                {
                    Id = Guid.NewGuid(),
                    EntityType = "Survey",
                    EntityId = survey.Id,
                    Rate = surveyDto.ClientRate,
                    // If you store Currency as string, you can put currency id or name here. Adjust as per your model.
                    Currency = surveyDto.Currency?.ToString(),
                    StartDate = startDate,
                    EndDate = null,
                    Note = "Initial survey rate",
                    CreatedBy = Guid.Parse(surveyDto.CreatedById),
                    CreatedAt = DateTime.UtcNow
                };
                await dbContext.RateHistory.AddAsync(surveyRate);

                // 5) Insert RateHistory for Partner (initial / linked rate)
                var partnerRate = new RateHistory
                {
                    Id = Guid.NewGuid(),
                    EntityType = "Partner",
                    EntityId = partnerSurvey.Id,
                    Rate = surveyDto.ClientRate,
                    Currency = surveyDto.Currency?.ToString(),
                    StartDate = startDate,
                    EndDate = null,
                    Note = $"Initial partner rate for survey {survey.Id}",
                    CreatedBy = Guid.Parse(surveyDto.CreatedById),
                    CreatedAt = DateTime.UtcNow
                };
                await dbContext.RateHistory.AddAsync(partnerRate);

                // save changes and commit
                await dbContext.SaveChangesAsync();
                await tx.CommitAsync();

                return survey_id;
            }
            catch (Exception ex)
            {
                // Optionally log the error here
                return ex.Message;
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
                survey.UniqueLink = surveyDto.UniqueLink;

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
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == surveyAddPartnerDto.SurveyUuid);

                PartnerSurvey partnerSurvey = new PartnerSurvey();
                partnerSurvey.Id = new Guid();
                partnerSurvey.PartnerId = surveyAddPartnerDto.PartnerId;
                partnerSurvey.AvailableVariable = surveyAddPartnerDto.AvailableVariable;
                //partnerSurvey.Rate = surveyAddPartnerDto.Rate;
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

                // Insert new rate
                var newRate = new RateHistory
                {
                    Id = Guid.NewGuid(),
                    EntityType = "Partner",
                    EntityId = partnerSurvey.Id,
                    Rate = surveyAddPartnerDto.Rate,
                    Currency = survey.CurrencyId,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    Note = "Partner added",
                    CreatedBy = null,
                    CreatedAt = DateTime.UtcNow
                };

                await dbContext.AddAsync(newRate);
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
        public async Task<SurveyResponseResultDto> SurveyAddResponseAsync(SurveyResponseDto surveyResponseDto)
        {
            SurveyResponseResultDto surveyResponseResultDto = new SurveyResponseResultDto();
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
                    surveyResponseResultDto.Status = "already exists";
                    surveyResponseResultDto.ResponseUuid = existingResponse.Id;
                    surveyResponseResultDto.ResponseLink = existingResponse.ClientURL;
                    surveyResponseResultDto.Passcode = existingResponse.passcode;
                }

                var Survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(s => s.Id == survey_id);

                if (Survey.UniqueLink == false)
                {
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
                        Status = "incomplete",
                        passcode = Guid.NewGuid().ToString(),
                        userIdFor = surveyResponseDto.addedby,

                    };

                    // Replace placeholder in client link
                    string updatedLink = Regex.Replace(survey_link, @"[\{\[\<][^}\]>]+[\}\]\>]", surveyResponseDto.RespondentId.ToString());

                    surveyResponse.ClientURL = updatedLink;

                    // Save to database
                    await dbContext.AddAsync(surveyResponse);
                    await dbContext.SaveChangesAsync();

                    surveyResponseResultDto.Status = "created";
                    surveyResponseResultDto.ResponseUuid = surveyResponse.RespondentId.ToString();
                    surveyResponseResultDto.ResponseLink = updatedLink;
                    surveyResponseResultDto.Passcode = surveyResponse.passcode;
                }
                else
                {
                    // get survey link from database
                    var surveyResponse = await dbContext.surveyResponses
                            .Where(sr => sr.SurveyId == survey_id && sr.userIdFor == null)
                            .OrderBy(sr => sr.CreatedAt)
                            .FirstOrDefaultAsync();

                    if (surveyResponse != null)
                    {
                        // Update the existing response with new details
                        surveyResponse.userIdFor = surveyResponseDto.addedby;
                        surveyResponse.ResponseDate = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync();

                        surveyResponseResultDto.Status = "created";
                        surveyResponseResultDto.ResponseUuid = surveyResponse.RespondentId.ToString();
                        surveyResponseResultDto.ResponseLink = surveyResponse.ClientURL;
                        surveyResponseResultDto.Passcode = surveyResponse.passcode;

                    }
                    else
                    {
                        surveyResponseResultDto.Status = "Not Available";
                        surveyResponseResultDto.ResponseUuid = "";
                        surveyResponseResultDto.ResponseLink = "";
                        surveyResponseResultDto.Passcode = "";
                    }
                }
                return surveyResponseResultDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SurveyAddResponseAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<object> SurveyResponseVerifyAsync(SurveyVerifyResponseDto surveyResponseDto)
        {
            try
            {
                // Fetch SurveyId and PartnerId from PartnerSurveys table
                var surveyResponse1 = await dbContext.surveyResponses
                    .FirstOrDefaultAsync(ps => ps.RespondentId == surveyResponseDto.RespondentId && ps.SurveyId.ToString() == surveyResponseDto.surveyId && ps.passcode == surveyResponseDto.PassCode && ps.Status == "incomplete");

                if (surveyResponse1 == null)
                {
                    throw new Exception($"No entry found in Id {surveyResponseDto.RespondentId}");
                }

                surveyResponse1.RespondentIP = surveyResponseDto.RespondentIP;
                surveyResponse1.ResponseDate = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();

                return new
                {
                    status = "verified",
                    response_link = surveyResponse1.ClientURL,
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
                        .FirstOrDefaultAsync(sr => sr.RespondentId.ToLower() == uid.ToLower());

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
                        surveyResponse.UpdatedAt = DateTime.UtcNow;
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
                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(sr => sr.Id.ToString().ToLower() == surveyId.ToLower());

                if (survey != null && survey.PreScreener)
                {
                    bool hasPreScreeners = await dbContext.SurveyPreScreeners
                        .AnyAsync(ps => ps.SurveyId.ToLower() == surveyId.ToLower());

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
                    UniqueLink = survey.UniqueLink,
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
                    Name = GenerateNextSurveyName(survey.Name),
                    //Name = "PDR" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString(),
                    Status = dbContext.MultiSelects
                        .Where(m => m.SelectionType == "status" && m.Name == "draft")
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
                    ParentId = survey.Id,
                    PreScreener = survey.PreScreener,
                    UniqueLink = survey.UniqueLink
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
        public string GenerateNextSurveyName(string surveyName)
        {
            // 1. Get the survey by Id
            var survey = dbContext.Surveys.FirstOrDefault(s => s.Name == surveyName);
            if (survey == null)
                throw new Exception($"Survey with Name {surveyName} not found");

            // 2. Traverse up to the top-most parent
            var topParent = survey;
            while (topParent.ParentId != null)
            {
                var parentSurvey = dbContext.Surveys.FirstOrDefault(s => s.Id == topParent.ParentId);
                if (parentSurvey == null)
                    break; // safety stop if parent is missing
                topParent = parentSurvey;
            }

            string baseName = topParent.Name;

            // 3. Get all existing clone names for this base survey
            var existingNames = dbContext.Surveys
                .Where(s => s.Name.StartsWith(baseName + "CL"))
                .Select(s => s.Name)
                .ToList();

            // 4. Regex to extract numeric suffix after CL
            Regex regex = new Regex($@"^{Regex.Escape(baseName)}CL(\d+)$");

            int maxSuffix = 0;

            foreach (var name in existingNames)
            {
                Match match = regex.Match(name);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int suffix))
                {
                    maxSuffix = Math.Max(maxSuffix, suffix);
                }
            }

            // 5. Always return clone name with CL + next number
            return $"{baseName}CL{(maxSuffix + 1):D2}";
        }


        public async Task<PreScreenerSurveyDto> GetSurveyPreScreeningQuestAsync(string Id)
        {
            List<SurveyPreScreenerDto> PreScreener = new List<SurveyPreScreenerDto>();
            PreScreenerSurveyDto surveyPreScreenerDto = null; // Declare outside so it's accessible

            try
            {
                // Get the list of prescreener questions for the associated survey
                var questions = await dbContext.SurveyPreScreeners
                    .Where(q => q.SurveyId.ToLower() == Id.ToLower())
                    .ToListAsync();

                var survey = await dbContext.Surveys
                    .FirstOrDefaultAsync(q => q.Id.ToString().ToLower() == Id.ToLower());

                // Map each question to the DTO
                PreScreener = questions.Select(q => new SurveyPreScreenerDto
                {
                    Id = q.Id.ToString(),
                    SurveyId = q.SurveyId,
                    Question = q.Question,
                    QuestionType = q.QuestionType,
                    Option1 = q.Option1,
                }).ToList();

                surveyPreScreenerDto = new PreScreenerSurveyDto
                {
                    surveyPreScreenerDtos = PreScreener,
                    SurveyId = survey?.Id.ToString(),
                    SurveyName = survey?.Name,
                    SurveyTitle = survey?.Title,
                };
            }
            catch (Exception ex)
            {
                // Optionally log exception
                // e.g., _logger.LogError(ex, "Error fetching PreScreener Survey data");
            }

            return surveyPreScreenerDto;
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

        public async Task<SurveyFile> GetSurveyFileAsync(SurveyFileDto surveyFileDto, string default_partner)
        {
            SurveyFile surveyFile;
            /////////////////////////////////
            // Validate file type and size
            try
            {
                string allowedExtensions = ".csv,.xlsx,.xls";

                var fileExtension = Path.GetExtension(surveyFileDto.File.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new Exception($"File type not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
                }
                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");

                // Ensure directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, fileName);
                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await surveyFileDto.File.CopyToAsync(stream);
                }
                // Save file metadata to database (pseudo-code)
                surveyFile = new SurveyFile
                {
                    Id = Guid.NewGuid(),
                    SurveyId = surveyFileDto.SurveyId,
                    FileName = fileName,
                    FileName_show = surveyFileDto.File.FileName,

                    TotalLinks = 0, // Initialize as needed
                    UsedLinks = 0, // Initialize as needed
                    RemainingLinks = 0, // Initialize as needed
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = surveyFileDto.UploadedBy
                };
                await dbContext.AddAsync(surveyFile);
                await dbContext.SaveChangesAsync();

                string surveyFileId = surveyFile.Id.ToString();

                // access file and iterate over loop
                // Step 2: Read CSV into DataTable
                DataTable dt = new DataTable("surveyResponses");

                // Add columns matching the table structure
                dt.Columns.Add("Id", typeof(string));                      // nvarchar(450)
                dt.Columns.Add("SurveyId", typeof(Guid));                  // uniqueidentifier
                dt.Columns.Add("RespondentId", typeof(string));            // nvarchar(max)
                dt.Columns.Add("ResponseDate", typeof(DateTime));          // datetime2(7)
                dt.Columns.Add("Status", typeof(string));                  // nvarchar(50)
                dt.Columns.Add("Answers", typeof(string));                 // nvarchar(max)        // nvarchar(max) nullable
                dt.Columns.Add("PartnerId", typeof(string));               // nvarchar(max) nullable
                dt.Columns.Add("CreatedAt", typeof(DateTime));             // datetime2(7)          // datetime2(7) nullable
                dt.Columns.Add("AddedBy", typeof(string));                 // nvarchar(max)
                dt.Columns.Add("ClientURL", typeof(string));               // nvarchar(max)
                dt.Columns.Add("passcode", typeof(string));                // nvarchar(max) nullable
                dt.Columns.Add("IsRecontact", typeof(bool));               // bit            
                dt.Columns.Add("SurveyFileId", typeof(Guid));              // uniqueidentifier nullable// uniqueidentifier nullable

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    bool firstRow = true;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Skip header if CSV has one
                        if (firstRow && line.Contains("http") == false)
                        {
                            firstRow = false;
                            continue;
                        }

                        // Example of how to add a row to the DataTable
                        DataRow row = dt.NewRow();

                        string[] final_url = ReplaceGuidPlaceholders(line.Trim());

                        row["Id"] = Guid.NewGuid();
                        row["SurveyId"] = surveyFileDto.SurveyId;
                        row["RespondentId"] = final_url[1];
                        row["ResponseDate"] = DateTime.UtcNow;
                        row["Status"] = "incomplete";
                        row["Answers"] = "";
                        row["PartnerId"] = default_partner;
                        row["CreatedAt"] = DateTime.UtcNow;
                        row["AddedBy"] = surveyFileDto.UploadedBy;
                        row["ClientURL"] = final_url[0];
                        row["passcode"] = Guid.NewGuid();
                        row["IsRecontact"] = false;
                        row["SurveyFileId"] = surveyFileId;

                        dt.Rows.Add(row);
                    }
                }
                string connectionString = _dataManager.GetConnectionString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Step 3: Bulk Insert into SurveyLinks
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = "surveyResponses";
                        bulkCopy.ColumnMappings.Add("Id", "Id");
                        bulkCopy.ColumnMappings.Add("SurveyId", "SurveyId");
                        bulkCopy.ColumnMappings.Add("RespondentId", "RespondentId");
                        bulkCopy.ColumnMappings.Add("ResponseDate", "ResponseDate");
                        bulkCopy.ColumnMappings.Add("Status", "Status");
                        bulkCopy.ColumnMappings.Add("Answers", "Answers");
                        bulkCopy.ColumnMappings.Add("PartnerId", "PartnerId");
                        bulkCopy.ColumnMappings.Add("CreatedAt", "CreatedAt");
                        bulkCopy.ColumnMappings.Add("AddedBy", "AddedBy");
                        bulkCopy.ColumnMappings.Add("ClientURL", "ClientURL");
                        bulkCopy.ColumnMappings.Add("passcode", "passcode");
                        bulkCopy.ColumnMappings.Add("IsRecontact", "IsRecontact");
                        bulkCopy.ColumnMappings.Add("SurveyFileId", "SurveyFileId");
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching survey data: " + ex.Message);
            }
            ///////////////////////////////////////////////
            return surveyFile;

        }
        public string[] ReplaceGuidPlaceholders(string url)
        {
            string varstring = Guid.NewGuid().ToString();
            string[] strings = new string[2];
            // Pattern to find any text within curly braces
            var regex = new Regex(@"\{([^}]+)\}");
            strings[0] = regex.Replace(url, match => varstring);
            strings[1] = varstring;

            // Replace all occurrences with GUIDs
            return strings;
        }
        public async Task<List<SurveyFile>> GetSurveyFilesAsync(string SurveyId)
        {
            List<SurveyFile> surveyFiles = new List<SurveyFile>();
            try
            {
                surveyFiles = await dbContext.surveyFile.Where(s => s.SurveyId.ToString().ToLower() == SurveyId.ToLower()).ToListAsync();
                foreach (var file in surveyFiles)
                {
                    string id = file.Id.ToString();
                    int totalCount = await dbContext.surveyResponses
                        .Where(sr => sr.SurveyFileId.ToString() == id)
                        .CountAsync();

                    // With the following code:
                    int usedCount = await dbContext.surveyResponses
                        .Where(sr => sr.SurveyFileId.ToString() == id && sr.userIdFor != null)
                        .CountAsync();

                    file.TotalLinks = totalCount; // Initialize as needed
                    file.UsedLinks = usedCount; // Initialize as needed
                    file.RemainingLinks = totalCount - usedCount; // Initialize as needed
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching survey data: " + ex.Message);
            }
            return surveyFiles;
        }
    }
}
