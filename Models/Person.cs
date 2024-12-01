using System.ComponentModel;

namespace MonthlyDataApi.Models
{
    public enum Status
    {
        [Description("")]
        zero,

        [Description("ראש קבוצה בבוקר")]
        f,

        [Description("ראש קבוצה אחה\"צ")]
        a,

        [Description("ראש כולל אחה\"צ והח' חצי דתות")]
        b,

        [Description("ראש כולל")]
        c,

        [Description("אברך רצופות יום שלם")]
        d,

        [Description("אברך רצופות חצי יום")]
        e
    }

    public enum Datot
    {
        [Description("לא רשום")]
        A,
        [Description("יום שלם")]
        B,
        [Description("חצי יום")]
        C
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Status Status { get; set; }
        public Datot Datot { get; set; }
        //public List<MonthlyRecord> MonthlyRecord { get; set; }
    }

    public class MonthlyRecord
    {
        public int PersonId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public bool isPresent { get; set; }
        public int BaseAllowance { get; set; }
        public Boolean IsChabura { get; set; }
        public Boolean DidLargeTest { get; set; }
        public int Datot { get; set; }
        public int TotalAmount { get; set;}

    }
}
