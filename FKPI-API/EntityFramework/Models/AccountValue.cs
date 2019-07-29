using System;

namespace FKPI_API.Models
{
    public class AccountValue
    {
        public int AccountValueId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal? Amount { get; set; }
        public int? Year { get; set; }
    }
}