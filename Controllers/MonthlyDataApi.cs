using Microsoft.AspNetCore.Mvc;
using MonthlyDataApi.DTOs;
using MonthlyDataApi.Models;
using MonthlyDataApi.Services;

namespace MonthlyDataApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlyDataController : ControllerBase
    {
        private readonly IMonthlyDataService _dataService;

        public MonthlyDataController(IMonthlyDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvrechim([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var avrechim = await _dataService.GetAvrechimAsync(page, pageSize);
            var totalAvrechim = (await _dataService.GetAvrechimAsync(1, int.MaxValue)).Count();

            return Ok(new AvrechimListDTO { Avrechim = avrechim.ToList(), TotalAvrechim = totalAvrechim });
        }

        [HttpGet("{id}/monthlydata")]
        public async Task<IActionResult> GetMonthlyData(int id, [FromQuery] string year, [FromQuery] string? month)
        {
            var monthlyData = await _dataService.GetMonthlyDataAsync(id, year, month);

            return Ok(monthlyData);
        }



        //[HttpPost]
        //public async Task<IActionResult> SaveData([FromBody] MonthlyRecord record)
        //{
        //    _dataService.SaveMonthlyRecord(record);
        //    return Ok();
        //}

        //[HttpGet("{month}")]
        //public async Task<IActionResult> GetDataForMonth([FromQuery] string month)
        //{
        //    var data = await _dataService.GetRecordsByMonth(month);
        //    return Ok(data);
        //}
    }
}
