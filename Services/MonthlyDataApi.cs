using Microsoft.EntityFrameworkCore;
using MonthlyDataApi.DTOs;
using MonthlyDataApi.Models;
using MonthlyDataApi.Models.MonthlyDataApi.Models;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonthlyDataApi.Services
{
    public interface IMonthlyDataService
    {
        Task<IEnumerable<AvrechDTO>> GetAvrechimAsync(int page, int pageSize);
        Task<IEnumerable<MonthlyRecordDTO>> GetMonthlyDataAsync(int id, string year, string month);
        Task<AvrechDTO> GetAvrechById(int id); // מתודה לקבלת פרטי אברך לפי מזהה
        Task DeleteAvrech(int id); // מתודה למחיקת אברך
        Task UpdateAvrech(int id, AvrechDTO avrechDTO); // מתודה לעדכון פרטי אברך
        Task AddAvrech(Person avrech); // מתודה להוספת אברך
        Task AddData(MonthlyRecord[] monthlyRecords);
        Task<IEnumerable<AvrechDTO>> SearchAvrechAsync(string query, string presence, string datot, string status);
        Task<IEnumerable<MonthlyRecordDTO>> GetLastMonthDataAsync(string year, string month);
        Task<IEnumerable<AvrechDTO>> getAvrechByIdAsync(int id);

    }

    public class MonthlyDataService : IMonthlyDataService
    {
        private readonly ApplicationDbContext _context;
        private MonthlyRecord monthlyData;

        public MonthlyDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AvrechDTO>> GetAvrechimAsync(int page, int pageSize)
        {
            var avrechim = await _context.Persons
                .OrderBy(p => p.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Status = p.Status.ToString().Replace("_"," "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent,
                    TeudatZeut = p.TeudatZeut,
                    DateOfBirth = p.DateOfBirth,
                    Phone= p.Phone,
                    CellPhone= p.CellPhone,
                    CellPhone2= p.CellPhone2,
                    Street= p.Street,
                    HouseNumber= p.HouseNumber,
                    Bank = p.Bank,
                    Branch = p.Branch,
                    AccountNumber= p.AccountNumber
                }).ToListAsync();

            var totalAvrechim = await _context.Persons.CountAsync();

            return avrechim;
        }

        public async Task<IEnumerable<MonthlyRecordDTO>> GetMonthlyDataAsync(int personId, string year, string? month)
        {
            var monthlyData = await _context.MonthlyRecords
                .Where(md => md.PersonId == personId && md.Year == year && (string.IsNullOrEmpty(month) || md.Month == month))
                .Select(md => new MonthlyRecordDTO
                {
                    PersonId = md.PersonId,
                    Year = md.Year,
                    Month = md.Month,
                    BaseAllowance = md.BaseAllowance,
                    IsChabura = md.IsChabura,
                    DidLargeTest = md.DidLargeTest,
                    Datot = md.Datot,
                    TotalAmount = md.TotalAmount,
                    OrElchanan = md.OrElchanan,
                    AddAmount = md.AddAmount,
                    Notes= md.Notes
                }).ToListAsync();

            return monthlyData;
        }

        public async Task<AvrechDTO> GetAvrechById(int id)
        {
            var avrech = await _context.Persons
                .Where(p => p.Id == id)
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Status = p.Status.ToString(),
                    Datot = p.Datot.ToString(),
                    isPresent = p.isPresent,
                    TeudatZeut = p.TeudatZeut,
                    DateOfBirth = p.DateOfBirth,
                    Phone = p.Phone,
                    CellPhone = p.CellPhone,
                    CellPhone2 = p.CellPhone2,
                    Street = p.Street,
                    HouseNumber = p.HouseNumber,
                    Bank = p.Bank,
                    Branch = p.Branch,
                    AccountNumber = p.AccountNumber
                })
                .FirstOrDefaultAsync();

            return avrech;
        }

        public async Task DeleteAvrech(int id)
        {
            var avrech = await _context.Persons.FindAsync(id);
            if (avrech != null)
            {
                var data = await _context.MonthlyRecords.Where(x => x.PersonId == id && x.Month == "Default").FirstOrDefaultAsync();
                if (data != null)
                {
                    _context.MonthlyRecords.Remove(data);
                }

                _context.Persons.Remove(avrech);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAvrech(int id, AvrechDTO avrechDTO)
        {
            var avrech = await _context.Persons.FindAsync(id);
            if (avrech != null)
            {
                avrech.FirstName = avrechDTO.FirstName;
                avrech.LastName = avrechDTO.LastName;
                avrech.Status = Enum.Parse<Status>(avrechDTO.Status);
                avrech.Datot = Enum.Parse<Datot>(avrechDTO.Datot);
                avrech.isPresent = avrechDTO.isPresent;
                avrech.Bank = avrechDTO.Bank;
                avrech.Branch = avrechDTO.Branch;
                avrech.AccountNumber = avrechDTO.AccountNumber;
                avrech.TeudatZeut = avrechDTO.TeudatZeut;
                avrech.Street = avrechDTO.Street;
                avrech.DateOfBirth= avrechDTO.DateOfBirth;
                avrech.Phone = avrechDTO.Phone;
                avrech.CellPhone= avrechDTO.CellPhone;
                avrech.CellPhone2= avrechDTO.CellPhone2;
                avrech.HouseNumber= avrechDTO.HouseNumber;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAvrech(Person avrech)
        {
            var existingAvrech = await _context.Persons
                .FirstOrDefaultAsync(p => p.TeudatZeut == avrech.TeudatZeut);

            if (existingAvrech != null)
            {
                throw new Exception("אברך עם מספר זהות זה כבר קיים");
            }
            _context.Persons.Add(avrech);
            await _context.SaveChangesAsync();

            if(avrech.Status == Status.ראש_כולל || avrech.Status == Status.אברך_רצופות_יום_שלם)
            {
                monthlyData = new MonthlyRecord
                {
                    PersonId = avrech.Id,
                    Month = "Default",
                    Year = "Default",
                    BaseAllowance = 3000,
                    IsChabura = false,
                    DidLargeTest = false,
                    Datot = 1000,
                    TotalAmount = 2000,
                    OrElchanan = 2000,
                    AddAmount = 0,
                    Notes = ""
                };
            }
            else
            {
                monthlyData = new MonthlyRecord
                {
                    PersonId = avrech.Id,
                    Month = "Default",
                    Year = "Default",
                    BaseAllowance = 1500,
                    IsChabura = false,
                    DidLargeTest = false,
                    Datot = 500,
                    TotalAmount = 1000,
                    OrElchanan = 1000,
                    AddAmount = 0,
                    Notes = ""
                };
            }
            
            _context.MonthlyRecords.Add(monthlyData);
            await _context.SaveChangesAsync();
        }

        public async Task AddData(MonthlyRecord[] monthlyRecords)
        {
            _context.MonthlyRecords.AddRangeAsync(monthlyRecords);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AvrechDTO>> SearchAvrechAsync(string query, string presence, string datot, string status)
        {
            //if (string.IsNullOrWhiteSpace(query))
            //{
            //    return Enumerable.Empty<AvrechDTO>();
            //}

            var avrechQuery = _context.Persons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                avrechQuery = avrechQuery.Where(p =>
                    EF.Functions.Like(p.FirstName, $"%{query}%") ||
                    EF.Functions.Like(p.LastName, $"%{query}%") ||
                    EF.Functions.Like(p.FirstName + " " + p.LastName, $"%{query}%")
                );
            }

            if (!string.IsNullOrWhiteSpace(presence))
            {
                avrechQuery = avrechQuery.Where(p => p.isPresent == presence);
            }

            if (!string.IsNullOrWhiteSpace(datot) && Enum.TryParse<Datot>(datot, out var datotEnum))
            {
                avrechQuery = avrechQuery.Where(p => p.Datot == datotEnum);
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Status>(status, out var statusEnum))
            {
                avrechQuery = avrechQuery.Where(p => p.Status == statusEnum);
            }

            //var totalItems = await avrechQuery.CountAsync();

            var avrech = await avrechQuery
 
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Status = p.Status.ToString().Replace("_", " "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent,
                    Bank = p.Bank,
                    Branch = p.Branch,
                    AccountNumber = p.AccountNumber,
                    Street = p.Street,
                    Phone = p.Phone,
                    CellPhone = p.CellPhone,
                    CellPhone2 = p.CellPhone2,
                    DateOfBirth = p.DateOfBirth,
                    HouseNumber = p.HouseNumber,
                    TeudatZeut = p.TeudatZeut
                })
                .ToListAsync();

            return avrech;
        }

        
        public async Task<IEnumerable<MonthlyRecordDTO>> GetLastMonthDataAsync(string? year, string? month)
        {
            var monthlyData = await _context.MonthlyRecords
                            .Where(md => (string.IsNullOrEmpty(year) || md.Year == year) && (string.IsNullOrEmpty(month) || md.Month == month))
                            .Select(md => new MonthlyRecordDTO
                            {
                                PersonId = md.PersonId,
                                Year = md.Year,
                                Month = md.Month,
                                BaseAllowance = md.BaseAllowance,
                                IsChabura = md.IsChabura,
                                DidLargeTest = md.DidLargeTest,
                                Datot = md.Datot,
                                TotalAmount = md.TotalAmount,
                                OrElchanan = md.OrElchanan,
                                AddAmount = md.AddAmount,
                                Notes= md.Notes
                            }).ToListAsync();

            return monthlyData;
        }

        public async Task<IEnumerable<AvrechDTO>> getAvrechByIdAsync(int id)
        {
            var avrech = await _context.Persons.Where(p => p.Id == id)
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Status = p.Status.ToString().Replace("_", " "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent,
                    TeudatZeut = p.TeudatZeut,
                    DateOfBirth = p.DateOfBirth,
                    Phone = p.Phone,
                    CellPhone = p.CellPhone,
                    CellPhone2 = p.CellPhone2,
                    Street = p.Street,
                    HouseNumber = p.HouseNumber,
                    Bank = p.Bank,
                    Branch = p.Branch,
                    AccountNumber = p.AccountNumber
                }).ToListAsync();

            return avrech;
        }
    }
}
