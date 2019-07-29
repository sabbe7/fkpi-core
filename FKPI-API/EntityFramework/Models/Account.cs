using System;
using System.Collections.Generic;

namespace FKPI_API.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int? ParentAccountId { get; set; }
        public Account ParentAccount { get; set; }
        public virtual ICollection<Account> Children { get; set; }
    }
}