using ABC.Models.DTO;

namespace ABC.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardOverviewDto> GetOverviewAsync();
        Task<List<TimePointDto>> GetResponsesTimeSeriesAsync(int days);
        Task<List<TopSurveyDto>> GetTopSurveysAsync(int limit);
        Task<List<RecentSurveyRowDto>> GetRecentSurveysAsync(int limit);
        Task<List<UpcomingSurveyDto>> GetUpcomingSurveysAsync(int limit);
        Task<List<AlertDto>> GetAlertsAsync();
    }
}
