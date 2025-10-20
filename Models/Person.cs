using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonthlyDataApi.Models
{
    namespace MonthlyDataApi.Models
    {
        public enum Status
        {
            None = 0,
            ראש_קבוצה_בבוקר = 1,
            ראש_קבוצה_אחה_צ = 2,
            ראש_כולל_אחה_צ_והח_חצי_דתות = 3,
            ראש_כולל = 4,
            יום_שלם = 5,
            _חצי_יום = 6

        }



        public enum Datot
        {
            לא_רשום = 1,
            יום_שלם = 2, 
            חצי_יום = 3
        }

        public class Person
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public Status Status { get; set; }
            public Datot Datot { get; set; }
            public string? isPresent { get; set; }
            public string? TeudatZeut { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Phone { get; set; }
            public string? CellPhone { get; set; }
            public string? CellPhone2 { get; set; }
            public string? Street { get; set; }
            public string? HouseNumber { get; set; }
            public string? Bank { get; set; }
            public string? Branch { get; set; }
            public string? AccountNumber { get; set; }
        }

        public class MonthlyRecord
        {
            public int Id { get; set; } 
            public int PersonId { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public int BaseAllowance { get; set; }
            public bool IsChabura { get; set; }
            public bool DidLargeTest { get; set; }
            public int Datot { get; set; }
            public int TotalAmount { get; set; }
            public int OrElchanan { get; set; }
            public int AddAmount { get; set; }
            public int Ginusar { get; set; }
            public string? Notes { get; set; }
        }
    }

}
