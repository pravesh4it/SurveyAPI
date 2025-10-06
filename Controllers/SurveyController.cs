using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyRepository surveyRepository;
        private readonly IMapper mapper;
        private readonly ClientSetting clientSetting;
        public SurveyController(ISurveyRepository surveyRepository, IMapper mapper, IOptions<ClientSetting> clientSettingOptions)
        {
            this.surveyRepository = surveyRepository;
            this.mapper = mapper;
            this.clientSetting = clientSettingOptions.Value;
        }

        [HttpGet("get-options-data")]
        public async Task<IActionResult> GetOptionsData()
        {
            var data = await surveyRepository.GetDataOptionsAsync();
            return Ok(data);
        }
        // POST: api/Survey/create-survey
        [HttpPost("create-survey")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyAddDto surveyDto)
        {
            if (surveyDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                surveyDto.DefaultPartner = clientSetting.Default;
                string survey_id = await surveyRepository.CreateAsync(surveyDto);
                // Check if survey_id is a valid GUID
                if (Guid.TryParse(survey_id, out Guid parsedSurveyId))
                {
                    return Ok(new { Message = "Survey added successfully", survey_id = parsedSurveyId });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to create survey. Invalid Survey ID." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        // POST: api/Survey/create-survey
        [HttpPut("update-survey/{id}")]
        public async Task<IActionResult> UpdateSurvey(Guid id, [FromBody] SurveyAddDto surveyDto)
        {
            if (surveyDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                string survey_id = await surveyRepository.UpdateAsync(id, surveyDto);
                // Check if survey_id is a valid GUID
                if (Guid.TryParse(survey_id, out Guid parsedSurveyId))
                {
                    return Ok(new { Message = "Survey updated successfully", survey_id = parsedSurveyId });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to update survey. Invalid Survey ID." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }

        [HttpPost("add-partner")]
        public async Task<IActionResult> SurveyAddPartner([FromBody] SurveyAddPartnerDto surveyAddPartnerDto)
        {
            if (surveyAddPartnerDto == null)
            {
                return BadRequest("Invalid survey partner data.");
            }
            try
            {
                string survey_id = await surveyRepository.SurveyAddPartnerAsync(surveyAddPartnerDto);
                // Check if survey_id is a valid GUID
                if (Guid.TryParse(survey_id, out Guid parsedSurveyId))
                {
                    return Ok(new { Message = "Survey partner added successfully", survey_id = parsedSurveyId });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to add survey partner." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }

        [HttpPost("add-surveyresponse")]
        public async Task<IActionResult> SurveyAddResponse([FromBody] SurveyResponseDto surveyResponseDto)
        {
            if (surveyResponseDto == null)
            {
                return BadRequest("Invalid survey partner data.");
            }

            try
            {
                SurveyResponseResultDto surveyResponseResultDto = await surveyRepository.SurveyAddResponseAsync(surveyResponseDto);
                if (surveyResponseResultDto.Status == "created")
                {
                    return Ok(surveyResponseResultDto); // success
                }
                else
                {
                    return BadRequest(new { message = surveyResponseResultDto.Status, details = surveyResponseResultDto.Status });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }

        [HttpPost("verify-surveyresponse")]
        public async Task<IActionResult> SurveyResponseVerifyAsync([FromBody] SurveyVerifyResponseDto surveyVerifyResponseDto)
        {
            if (surveyVerifyResponseDto == null)
            {
                return BadRequest("Invalid survey partner data.");
            }

            try
            {
                var responseobject = await surveyRepository.SurveyResponseVerifyAsync(surveyVerifyResponseDto);

                // You can cast it as dynamic to access properties without needing a class
                var status = ((dynamic)responseobject).status as string;

                if (status == "verified")
                {
                    return Ok(responseobject); // success
                }
                else
                {
                    return BadRequest(new { message = "You already participated", details = responseobject });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }


        [HttpGet("view-survey/{id}")]
        public async Task<IActionResult> GetSurveyClientPartners(Guid id)
        {
            var data = await surveyRepository.GetSurveyClientPartnersAsync(id);
            return Ok(data);
        }
        [HttpGet("survey-list")]
        public async Task<IActionResult> GetSurveyList()
        {
            var data = await surveyRepository.GetSurveyListAsync();
            return Ok(data);
        }
        [HttpGet("survey-partners-list/{id}")]
        public async Task<IActionResult> GetSurveyPartnersList(Guid id)
        {
            var data = await surveyRepository.GetSurveyPartnersListAsync(id);
            return Ok(data);
        }
        [HttpGet("partners-list")]
        public async Task<IActionResult> GetPartnersList()
        {
            var data = await surveyRepository.GetPartnersListAsync();
            return Ok(data);
        }
        [HttpPost("complete-surveyresponse")]
        public async Task<IActionResult> SurveyCompleteResponse([FromBody] SurveyCompleteResponseDto surveyCompleteResponseDto)
        {
            if (surveyCompleteResponseDto == null)
            {
                return BadRequest("Invalid survey partner data.");
            }
            try
            {
                var responseobject = await surveyRepository.SurveyCompleteResponseAsync(surveyCompleteResponseDto);
                // You can now access result like this:
                if (responseobject.Status == "Error")
                {
                    return BadRequest(responseobject.Message);
                }

                return Ok(responseobject.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        [HttpGet("survey-report")]
        public async Task<IActionResult> GetSurveyReport(string surveyid)
        {
            var data = await surveyRepository.GetSurveyReportAsync(surveyid);
            return Ok(data);
        }
        [HttpGet("edit-survey/{id}")]
        public async Task<IActionResult> GetSurveyById(Guid id)
        {
            var data = await surveyRepository.GetSurveyByIdAsync(id);
            return Ok(data);
        }

        // POST: api/Survey/create-survey
        [HttpPost("clone-survey")]
        public async Task<IActionResult> CloneSurvey([FromBody] CloneSurveyDto cloneSurveyDto)
        {
            if (cloneSurveyDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                string survey_id = await surveyRepository.CloneAsync(cloneSurveyDto);
                // Check if survey_id is a valid GUID
                if (Guid.TryParse(survey_id, out Guid parsedSurveyId))
                {
                    return Ok(new { Message = "Survey added successfully", survey_id = parsedSurveyId });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to create survey. Invalid Survey ID." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        // DELETE: api/Client/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await surveyRepository.DeleteAsync(id);
                if (!deleted)
                    return NotFound("Survey not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // POST: api/Survey/create-survey
        [HttpGet("survey-prescreening")]
        public async Task<IActionResult> GetSurveyPreScreening(string surveyid)
        {
            var data = await surveyRepository.GetSurveyPreScreeningAsync(surveyid);
            return Ok(data);
        }
        [HttpPost("survey-prescreening")]
        public async Task<IActionResult> AddPrescreening([FromBody] PreScreenerAddDto preScreenerAddDto)
        {
            if (preScreenerAddDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                var responseobject = await surveyRepository.SurveyAddPreScreenerAsync(preScreenerAddDto);
                // Check if survey_id is a valid GUID
                return Ok(responseobject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        // DELETE: api/Client/{id}
        [HttpGet("survey-is-prescreening")]
        public async Task<IActionResult> GetIsSurveyPreScreening(string surveyid)
        {
            var data = await surveyRepository.GetIsSurveyPreScreeningAsync(surveyid);
            return Ok(data);
        }
        [HttpGet("survey-prescreening-questions")]
        public async Task<IActionResult> GetSurveyPreScreeningQuestions(string id)
        {
            var data = await surveyRepository.GetSurveyPreScreeningQuestAsync(id);
            return Ok(data);
        }
        [HttpPost("survey-prescreening-questions")]
        public async Task<IActionResult> SurveyPreScreeningQuestions(string id)
        {
            var data = await surveyRepository.GetSurveyPreScreeningQuestAsync(id);
            return Ok(data);
        }

        [HttpPost("validate-prescreener")]
        public async Task<IActionResult> ValidatePreScreener([FromBody] SurveyQuestioinResponseDto survey)
        {
            if (survey == null || survey.Responses == null || !survey.Responses.Any())
                return BadRequest("Invalid survey data");

            bool hasDisqualifyingAnswer = await surveyRepository.HasDisqualifyingAnswerAsync(survey.Responses);

            if (hasDisqualifyingAnswer)
            {
                return BadRequest(new
                {
                    message = "User selected a disqualifying option ([x]) and is not allowed to continue."
                });
            }

            return Ok(new
            {
                message = "Survey passed. User can continue to the next step."
            });
        }

        [HttpPost("change-response-status")]
        public async Task<IActionResult> UpdateSurveyStatus([FromBody] UpdateSurveyStatusDto surveystatusdto)
        {
            if (surveystatusdto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                var responseobject = await surveyRepository.SurveyResponseUpdateStatusAsync(surveystatusdto);
                return Ok(responseobject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        [HttpPost("survey-recontact")]
        public async Task<IActionResult> RecontactSurvey([FromForm] RecontactDto recontactDto)
        {
            if (recontactDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                var responseobject = await surveyRepository.AddRecontact(recontactDto);
                return Ok(responseobject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }
        [HttpGet("get-survey-name/{survey_partner_id}")]
        public async Task<IActionResult> GetSurveyByParnerId(Guid survey_partner_id)
        {
            var data = await surveyRepository.GetSurveyByPartnerIdAsync(survey_partner_id);
            return Ok(data);
        }
        // POST: api/Survey/create-survey
        [HttpPost("update-survey-status/{id}")]
        public async Task<IActionResult> UpdateSurveyStatus(Guid id, [FromBody] SurveyStatusUpdateDto surveyStatusUpdateDto)
        {
            if (surveyStatusUpdateDto == null)
            {
                return BadRequest("Invalid survey data.");
            }
            try
            {
                string survey_id = await surveyRepository.UpdateStatusAsync(id, surveyStatusUpdateDto);
                // Check if survey_id is a valid GUID
                if (Guid.TryParse(survey_id, out Guid parsedSurveyId))
                {
                    return Ok(new { Message = "Survey updated successfully", survey_id = parsedSurveyId });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to update survey. Invalid Survey ID." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving data: {ex.Message}");
            }
        }

        [HttpPost("upload-survey-csv")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] SurveyFileDto surveyFileDto)
        {
            if (surveyFileDto == null || surveyFileDto.File == null)
            {
                return BadRequest("Invalid file data.");
            }

            try
            {
                string default_partner = clientSetting.Default;
                var responseObject = await surveyRepository.GetSurveyFileAsync(surveyFileDto, default_partner);
                return Ok(responseObject);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpGet("survey-csvfile/{id}")]
        public async Task<IActionResult> GetSurveyFiles(string id)
        {
            var data = await surveyRepository.GetSurveyFilesAsync(id);
            return Ok(data);
        }
    }
}
