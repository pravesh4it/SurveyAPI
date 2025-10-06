using ABC.Models.Domain;
using ABC.Models.DTO;
using System.Threading.Tasks;

namespace ABC.Repositories
{
    public interface ISurveyRepository
    {
        Task<object> GetDataOptionsAsync();
        Task<string> CreateAsync(SurveyAddDto surveyAddDto);
        Task<string> UpdateAsync(Guid Id, SurveyAddDto surveyDto);
        Task<bool> DeleteAsync(Guid clientId);
        Task<Client> GetClentAsync(Guid clientId);
        Task<string> SurveyAddPartnerAsync(SurveyAddPartnerDto surveyAddPartnerDto);
        Task<SurveyViewDto> GetSurveyClientPartnersAsync(Guid Id);
        Task<List<SurveyDto>> GetSurveyListAsync();
        Task<List<PartnerSurveyDto>> GetSurveyPartnersListAsync(Guid Id);
        Task<object> GetPartnersListAsync();
        Task<SurveyResponseResultDto> SurveyAddResponseAsync(SurveyResponseDto surveyResponseDto);
        Task<SurveyResponseResult> SurveyCompleteResponseAsync(SurveyCompleteResponseDto surveyCompleteResponseDto);
        Task<object> GetSurveyReportAsync(string surveyId);
        Task<SurveyEditDto> GetSurveyByIdAsync(Guid surveyId);
        Task<string> CloneAsync(CloneSurveyDto cloneSurveyDto);
        Task<object> GetSurveyPreScreeningAsync(string surveyId);
        Task<object> GetIsSurveyPreScreeningAsync(string surveyId);
        Task<PreScreenerSurveyDto> GetSurveyPreScreeningQuestAsync(string Id);
        Task<object> SurveyAddPreScreenerAsync(PreScreenerAddDto preScreenerAddDto);
        Task<bool> HasDisqualifyingAnswerAsync(List<ResponseDto> responses);
        Task<bool> SurveyResponseUpdateStatusAsync(UpdateSurveyStatusDto surveystatusDto);
        Task<string> AddRecontact(RecontactDto recontactDto);
        Task<SurveyEditDto> GetSurveyByPartnerIdAsync(Guid survey_partner_id);
        Task<string> UpdateStatusAsync(Guid surveyId, SurveyStatusUpdateDto surveyStatusUpdateDto);
        Task<object> SurveyResponseVerifyAsync(SurveyVerifyResponseDto surveyResponseDto);
        Task<SurveyFile> GetSurveyFileAsync(SurveyFileDto surveyFileDto, string default_partner);
        Task<List<SurveyFile>> GetSurveyFilesAsync(string SurveyId);

    }
}
