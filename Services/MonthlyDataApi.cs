// Services/MonthlyDataService.cs
using MonthlyDataApi.DTOs;
using MonthlyDataApi.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace MonthlyDataApi.Services
{
    public interface IMonthlyDataService
    {
        //void SaveMonthlyRecord(MonthlyRecord record);
        //List<MonthlyRecord> GetRecordsByMonth(string month);
        Task<IEnumerable<AvrechDTO>> GetAvrechimAsync(int page, int pageSize);
        Task<IEnumerable<MonthlyRecordDTO>> GetMonthlyDataAsync(int id, string year, string month);


    }

    public class MonthlyDataService : IMonthlyDataService
    {
        private List<MonthlyRecord> _monthlyData;
        private readonly List<Person> _avrechim;

        public MonthlyDataService()
        {
            _avrechim = new List<Person>
            {
                new Person{ Id = 1, FirstName = "צבי", LastName = "ארזי", Status = Status.zero, Datot = Datot.A
 
                },
                new Person{ Id = 2, FirstName = "נחום", LastName = "ברונשטיין", Status = Status.d, Datot = Datot.B

                },
                new Person{ Id = 3, FirstName = "יהודה", LastName = "גינסברגר", Status = Status.d, Datot= Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount  = 2300},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount = 2300}
                //}
                },
                new Person{ Id = 4, FirstName = "אברהם", LastName = "דייטש", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List < MonthlyRecord >
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount  = 2300},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount = 2300}
                //}
                },
                new Person{ Id = 5, FirstName = "יוסף", LastName = "הלוי", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount  = 2300},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount = 2300}
                //}
                },
                new Person{ Id = 6, FirstName = "מרדכי", LastName = "ויס", Status = Status.d, Datot = Datot.B,
                //    MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount  = 2000},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000}
                //}
                },
                new Person{ Id = 7, FirstName = "מרדכי אבינועם", LastName = "יעקובסון", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount  = 2000},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000}
                //}
                },
                new Person{ Id = 8, FirstName = "חיים", LastName = "לפקוביץ", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount  = 2000},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000}
                //}
                },
                new Person{ Id = 9, FirstName = "חיים יצחק", LastName = "מאיר", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 2800, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount  = 1800},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 2800, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 1800}
                //}
                },
                new Person{ Id = 10, FirstName = "רפאל", LastName = "סופר", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount  = 2300},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount = 2300}
                //}
                },
                new Person{ Id = 12, FirstName = "יעקב", LastName = "עמר", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 2800, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount  = 2100},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 2800, IsChabura = true, DidLargeTest = false, Datot = 1000, TotalAmount = 2100}
                //}
                },
                new Person{ Id = 13, FirstName = "יהושע", LastName = "פורייס", Status = Status.d, Datot = Datot.B
                //, MonthlyRecord = new List<MonthlyRecord>
                //{
                //    new MonthlyRecord{ Month = "חשוון", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount  = 2000},
                //    new MonthlyRecord{ Month = "תשרי", Year = "תשפ\"ה", BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000}
                //}
                }
            };

            _monthlyData = new List<MonthlyRecord>
            {
                new MonthlyRecord { PersonId = 1, Month = "חשוון", Year = "תשפ\"ה", isPresent = true, BaseAllowance = 0, IsChabura = true, DidLargeTest = false, Datot = 0, TotalAmount = 300},
                new MonthlyRecord{ PersonId = 1, Month = "תשרי", Year = "תשפ\"ה", isPresent = true, BaseAllowance = 0, IsChabura = true, DidLargeTest = false, Datot = 0, TotalAmount = 300},
                new MonthlyRecord{ PersonId = 2, Month = "חשוון", Year = "תשפ\"ה", isPresent = true, BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000},
                new MonthlyRecord{ PersonId = 2, Month = "תשרי", Year = "תשפ\"ה", isPresent = true, BaseAllowance = 3000, IsChabura = false, DidLargeTest = false, Datot = 1000, TotalAmount = 2000}
            };
        }

        //public void SaveMonthlyRecord(MonthlyRecord record)
        //{
        //    _records.Add(record);
        //}

        //public List<MonthlyRecord> GetRecordsByMonth(string month)
        //{
        //    return _records.FindAll(r => r.Month == month);
        //}


        public Task<IEnumerable<AvrechDTO>> GetAvrechimAsync(int page, int pageSize)
        {
            var avrechim = _avrechim
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new AvrechDTO
                {
                    Id = r.Id,
                    FullName = $"{r.FirstName} {r.LastName}",
                    Status = GetEnumDescription(r.Status),
                    Datot = GetEnumDescription(r.Datot)
                }).ToList();
            return Task.FromResult(avrechim.AsEnumerable());
        }

        public Task<IEnumerable<MonthlyRecordDTO>> GetMonthlyDataAsync(int personId, string year, string? month)
        {
            var monthlyData = _monthlyData
                .Where(md => md.PersonId == personId && md.Year == year && (string.IsNullOrEmpty(month) || md.Month == month))
                .Select(md => new MonthlyRecordDTO
                {
                    PersonId = md.PersonId,
                    Year = md.Year,
                    Month = md.Month,
                    isPresent = md.isPresent,
                    BaseAllowance = md.BaseAllowance,
                    IsChabura = md.IsChabura,
                    DidLargeTest = md.DidLargeTest,
                    Datot = md.Datot,
                    TotalAmount = md.TotalAmount
                }).ToList();

            return Task.FromResult(monthlyData.AsEnumerable());
        }


        private string GetEnumDescription<T>(T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : enumValue.ToString(); // אם לא נמצא תיאור, מחזיר את שם הערך
        }

    }
}
