﻿using Microsoft.EntityFrameworkCore;
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

        public MonthlyDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AvrechDTO>> GetAvrechimAsync(int page, int pageSize)
        {
            var avrechim = await _context.Persons
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}",
                    Status = p.Status.ToString().Replace("_"," "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent
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
                    TotalAmount = md.TotalAmount
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
                    FullName = $"{p.FirstName} {p.LastName}",
                    Status = p.Status.ToString(),
                    Datot = p.Datot.ToString(),
                    isPresent = p.isPresent
                })
                .FirstOrDefaultAsync();

            return avrech;
        }

        public async Task DeleteAvrech(int id)
        {
            var avrech = await _context.Persons.FindAsync(id);
            if (avrech != null)
            {
                _context.Persons.Remove(avrech);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAvrech(int id, AvrechDTO avrechDTO)
        {
            var avrech = await _context.Persons.FindAsync(id);
            if (avrech != null)
            {
                avrech.FirstName = avrechDTO.FullName.Split(' ')[0];
                avrech.LastName = avrechDTO.FullName.Split(' ')[1];
                avrech.Status = Enum.Parse<Status>(avrechDTO.Status);
                avrech.Datot = Enum.Parse<Datot>(avrechDTO.Datot);
                avrech.isPresent = avrechDTO.isPresent;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAvrech(Person avrech)
        {
            _context.Persons.Add(avrech);
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
                    FullName = $"{p.FirstName} {p.LastName}",
                    Status = p.Status.ToString().Replace("_", " "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent
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
                                TotalAmount = md.TotalAmount
                            }).ToListAsync();

            return monthlyData;
        }

        public async Task<IEnumerable<AvrechDTO>> getAvrechByIdAsync(int id)
        {
            var avrech = await _context.Persons.Where(p => p.Id == id)
                .Select(p => new AvrechDTO
                {
                    Id = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}",
                    Status = p.Status.ToString().Replace("_", " "),
                    Datot = p.Datot.ToString().Replace("_", " "),
                    isPresent = p.isPresent
                }).ToListAsync();

            return avrech;
        }
    }
}
