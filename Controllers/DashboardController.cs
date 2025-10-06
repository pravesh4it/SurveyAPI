using ABC.Models.Domain;
using ABC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardRepository dashboardRepository;
        private readonly ClientSetting clientSetting;
        public DashboardController(IDashboardRepository dashboardRepository, IOptions<ClientSetting> clientSettingOptions)
        {
            this.dashboardRepository = dashboardRepository;
            this.clientSetting = clientSettingOptions.Value;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> Overview()
        {
            var data = await dashboardRepository.GetOverviewAsync();
            return Ok(data);
        }

        [HttpGet("timeseries")]
        public async Task<IActionResult> TimeSeries([FromQuery] int days = 14)
        {
            if (days < 1) days = 1;
            if (days > 90) days = 90;
            var data = await dashboardRepository.GetResponsesTimeSeriesAsync(days);
            return Ok(data);
        }

        [HttpGet("top")]
        public async Task<IActionResult> Top([FromQuery] int limit = 5)
        {
            if (limit < 1) limit = 1;
            if (limit > 20) limit = 20;
            var data = await dashboardRepository.GetTopSurveysAsync(limit);
            return Ok(data);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> Recent([FromQuery] int limit = 10)
        {
            if (limit < 1) limit = 1;
            if (limit > 50) limit = 50;
            var data = await dashboardRepository.GetRecentSurveysAsync(limit);
            return Ok(data);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming([FromQuery] int limit = 8)
        {
            var data = await dashboardRepository.GetUpcomingSurveysAsync(limit);
            return Ok(data);
        }

        [HttpGet("alerts")]
        public async Task<IActionResult> Alerts()
        {
            var data = await dashboardRepository.GetAlertsAsync();
            return Ok(data);
        }
    }
}
