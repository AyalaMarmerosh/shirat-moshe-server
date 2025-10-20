namespace MonthlyDataApi.DTOs
{
    public class MonthlyRecordDTO
    {
        public int? Id { get; set; }
        public int PersonId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int BaseAllowance { get; set; }
        public Boolean IsChabura { get; set; }
        public Boolean DidLargeTest { get; set; }
        public int Datot { get; set; }
        public int TotalAmount { get; set; }
        public int OrElchanan { get; set; }
        public int AddAmount { get; set; }
        public int? Ginusar { get; set; }
        public string? Notes { get; set; }
    }
}
