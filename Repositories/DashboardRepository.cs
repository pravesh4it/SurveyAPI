using ABC.Data;
using ABC.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AbcDbContext dbContext;

        public DashboardRepository(AbcDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DashboardOverviewDto> GetOverviewAsync()
        {
            var nowUtc = DateTime.UtcNow;
            var totalSurveys = await dbContext.Surveys.CountAsync();
            var activeCount = await (
                    from s in dbContext.Surveys
                    where dbContext.MultiSelects
                    .Any(ms => ms.Id.ToString().ToLower() == s.Status.ToLower()
                    && ms.SelectionType == "status"
                    && ms.Name != "Closed"
                    && ms.Name != "Draft")
                    select s
                    ).CountAsync();

            var DraftCount = await (
                    from s in dbContext.Surveys
                    where dbContext.MultiSelects
                    .Any(ms => ms.Id.ToString().ToLower() == s.Status.ToLower()
                    && ms.SelectionType == "status"
                    && ms.Name == "Draft")
                    select s
                    ).CountAsync();

            // Responses
            var totals = await dbContext.surveyResponses
                .GroupBy(r => 1)
                .Select(g => new
                {
                    total = g.Count(),
                    completes = g.Count(r => r.Status == "success")
                })
                .FirstOrDefaultAsync() ?? new { total = 0, completes = 0 };

            // Response Rate: completes / started (exclude never-started)
            // Consider "started" as all except null (or count all records)
            var started = totals.total;
            double responseRate = started == 0 ? 0 : (totals.completes * 100.0 / started);
            // Status distribution

            StatusCountDto statusCountDtoClosed = new StatusCountDto();
            statusCountDtoClosed.Status = "Closed";
            statusCountDtoClosed.Count = totalSurveys - (activeCount + DraftCount);

            StatusCountDto statusCountDtoActive = new StatusCountDto();
            statusCountDtoActive.Status = "Active";
            statusCountDtoActive.Count = activeCount;

            StatusCountDto statusCountDtoDraft = new StatusCountDto();
            statusCountDtoDraft.Status = "Draft";
            statusCountDtoDraft.Count = DraftCount;

            List<StatusCountDto> statusCounts = new List<StatusCountDto>();
            statusCounts.Add(statusCountDtoActive);
            statusCounts.Add(statusCountDtoClosed);
            statusCounts.Add(statusCountDtoDraft);


            return new DashboardOverviewDto
            {
                TotalSurveys = totalSurveys,
                ActiveSurveys = activeCount,
                ClosedSurveys = totalSurveys - (activeCount + DraftCount),
                TotalResponses = totals.completes, // show completes; change if you want all
                ResponseRate = Math.Round(responseRate, 2),
                StatusDistribution = statusCounts
            };
        }

        public async Task<List<TimePointDto>> GetResponsesTimeSeriesAsync(int days)
        {
            var end = DateTime.UtcNow.Date;             // today 00:00 UTC
            var start = end.AddDays(-days + 1);         // inclusive window

            // Group completes per day (UTC)
            var raw = await dbContext.surveyResponses
                .Where(r => r.Status == "success" && r.UpdatedAt >= start && r.UpdatedAt < end.AddDays(1))
                .GroupBy(r => r.UpdatedAt!.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            // Fill gaps for days with zero completes
            var dict = raw.ToDictionary(x => x.Date, x => x.Count);
            var series = new List<TimePointDto>();
            for (var d = 0; d < days; d++)
            {
                var day = start.AddDays(d);
                series.Add(new TimePointDto
                {
                    Date = day,
                    Count = dict.TryGetValue(day, out var cnt) ? cnt : 0
                });
            }
            return series;
        }

        public async Task<List<TopSurveyDto>> GetTopSurveysAsync(int limit)
        {
            return await dbContext.surveyResponses
                .Where(r => r.Status == "complete")
                .GroupBy(r => r.SurveyId)
                .Select(g => new
                {
                    SurveyId = g.Key,
                    Responses = g.Count()
                })
                .OrderByDescending(x => x.Responses)
                .Take(limit)
                .Join(dbContext.Surveys,
                      x => x.SurveyId,
                      s => s.Id,
                      (x, s) => new TopSurveyDto
                      {
                          SurveyId = s.Id,
                          Name = s.Name,
                          Responses = x.Responses
                      })
                .ToListAsync();
        }

        public async Task<List<RecentSurveyRowDto>> GetRecentSurveysAsync(int limit)
        {
            // latest created surveys with their completes
            var q = dbContext.Surveys
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    //s.Status,
                    s.CreatedOn,
                    s.EndDate,
                    s.Title,
                    Completes = dbContext.surveyResponses.Count(r => r.SurveyId == s.Id && r.Status == "success"),
                    StatusText = dbContext.MultiSelects.FirstOrDefault(m => m.Id.ToString().ToLower() == s.Status.ToString().ToLower()).Name
                })
                .Take(limit);

            var data = await q.ToListAsync();
            return data.Select(s => new RecentSurveyRowDto
            {
                SurveyId = s.Id,
                Name = s.Name,
                Status = s.StatusText,
                CreatedAt = s.CreatedOn,
                EndAt = s.EndDate == null ? null : s.EndDate,
                Responses = s.Completes,
                Owner = s.Title
            }).ToList();
        }

        public async Task<List<UpcomingSurveyDto>> GetUpcomingSurveysAsync(int limit)
        {
            var now = DateTime.UtcNow;
            return await dbContext.Surveys
                .Where(s => s.Status == "Scheduled" && s.CreatedOn != null && s.CreatedOn > now)
                .OrderBy(s => s.CreatedOn)
                .Take(limit)
                .Select(s => new UpcomingSurveyDto
                {
                    SurveyId = s.Id,
                    Name = s.Name,
                    StartAt = s.CreatedOn,
                    Owner = s.Title
                })
                .ToListAsync();
        }

        public async Task<List<AlertDto>> GetAlertsAsync()
        {
            var now = DateTime.UtcNow;
            var soon = now.AddDays(7);

            var expiring = await dbContext.Surveys
                .Where(s => s.Status == "Active" && s.EndDate != null && Convert.ToDateTime(s.EndDate) <= soon)
                .Select(s => new AlertDto
                {
                    Type = "warning",
                    Text = $"{s.Name} ends on {s.EndDate!.Value:yyyy-MM-dd}."
                })
                .ToListAsync();

            // Low response rate (last 7 days): completes/(all results) < 5%
            var since = now.AddDays(-7);
            var totals = await dbContext.surveyResponses
                .Where(r => r.UpdatedAt >= since)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    total = g.Count(),
                    completes = g.Count(r => r.Status == "success")
                })
                .FirstOrDefaultAsync();

            var alerts = new List<AlertDto>();
            alerts.AddRange(expiring);

            if (totals != null && totals.total >= 50) // threshold to avoid noise
            {
                var rate = totals.completes * 100.0 / totals.total;
                if (rate < 5)
                {
                    alerts.Add(new AlertDto
                    {
                        Type = "info",
                        Text = $"Low response rate in last 7 days: {rate:F1}%."
                    });
                }
            }

            return alerts;
        }
    }
}
