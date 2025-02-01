using MonthlyDataApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonthlyDataApi.DTOs
{
    public class AvrechDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Status { get; set; } 
        public string Datot { get; set; }
        public string isPresent { get; set; }
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

        //public List<MonthlyRecordDTO> MonthlyRecord { get; set; 

    }
}
