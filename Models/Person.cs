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
            אברך_רצופות_יום_שלם = 5,
            אברך_רצופות_חצי_יום = 6
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
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Status Status { get; set; }
            public Datot Datot { get; set; }
            public string isPresent { get; set; }
        }

        public class MonthlyRecord
        {
            public int Id { get; set; } // חובה להוסיף ID
            public int PersonId { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public int BaseAllowance { get; set; }
            public bool IsChabura { get; set; }
            public bool DidLargeTest { get; set; }
            public int Datot { get; set; }
            public int TotalAmount { get; set; }
        }
    }

}
