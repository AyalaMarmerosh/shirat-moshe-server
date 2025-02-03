using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonthlyDataApi.DTOs;
using MonthlyDataApi.Models;
using MonthlyDataApi.Models.MonthlyDataApi.Models;
using MonthlyDataApi.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonthlyDataApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlyDataController : ControllerBase
    {
        private readonly IMonthlyDataService _dataService;
        private readonly LoginService _loginService;

        public MonthlyDataController(IMonthlyDataService dataService, LoginService loginService)
        {
            _dataService = dataService;
            _loginService = loginService;
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

        [HttpGet("{id}/avrech")]
        public async Task<IActionResult> getAvrechById(int id)
        {
            var avrech = await _dataService.getAvrechByIdAsync(id);
            return Ok(avrech);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            Console.WriteLine($"זה ניסיום: {model.Username}");

            try
            {
                Console.WriteLine($"Login attempt for username: {model.Username}");
                var token = _loginService.Login(model.Username, model.Password);
                Console.WriteLine($"Generated JWT for user: {model.Username}");
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("update-credentials")]
        public IActionResult UpdateCredentials([FromBody] UpdateCredentialsModel model)
        {
            try
            {
                _loginService.UpdateCredentials(model.OldUsername, model.NewUsername, model.NewPassword);
                return Ok("User credentials updated successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class UpdateCredentialsModel
        {
            public string OldUsername { get; set; }
            public string NewUsername { get; set; }
            public string NewPassword { get; set; }
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddAvrech([FromBody] Person avrech)
        {
            try
            {
                await _dataService.AddAvrech(avrech);
                return Ok(avrech);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addData")]
        public async Task<IActionResult> AddData([FromBody] MonthlyRecord[] monthlyRecords)
        {
            try
            {
                await _dataService.AddData(monthlyRecords);
                return Ok(monthlyRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvrech(int id)
        {
            var avrech = await _dataService.GetAvrechById(id);
            if (avrech == null)
            {
                return NotFound("Avrech not found");
            }

            await _dataService.DeleteAvrech(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvrech(int id, [FromBody] AvrechDTO avrechDTO)
        {
            var avrech = await _dataService.GetAvrechById(id);
            if (avrech == null)
            {
                return NotFound("Avrech not found");
            }

            await _dataService.UpdateAvrech(id, avrechDTO);
            return Ok(avrechDTO);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAvrech([FromQuery] string query = "", [FromQuery] string presence = "", [FromQuery] string datot = "", [FromQuery] string status = "")
        {
            var avrech = await _dataService.SearchAvrechAsync(query, presence, datot, status);

            return Ok(avrech.ToList());
        }

        [HttpGet("last-month")]
        public async Task<IActionResult> GetLastMonthData([FromQuery] string? year, [FromQuery] string? month)
        {
            try
            {
                var lastMonthData = await _dataService.GetLastMonthDataAsync(year, month);

                return Ok(lastMonthData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בשרת: {ex.Message}");
            }
        }

    }

}
