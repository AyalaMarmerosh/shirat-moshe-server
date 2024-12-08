using MonthlyDataApi.Models;

namespace MonthlyDataApi.DTOs
{
    public class AvrechDTO
    {
        public int Id { get; set; }        
        public string FullName { get; set; } // שילוב של שם פרטי ושם משפחה
        public string Status { get; set; } // ייתכן שתצטרך להמיר את המצב למחרוזת לקריאה נוחה
        public string Datot { get; set; }
        public string isPresent { get; set; }

        //public List<MonthlyRecordDTO> MonthlyRecord { get; set; 

    }
}
