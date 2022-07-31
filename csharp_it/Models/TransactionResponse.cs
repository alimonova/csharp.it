using System;
namespace csharp_it.Models
{
    public class TransactionResponse
    {
        public string reason { get; set; }
        public int reasonCode { get; set; }
        public Transaction[] transactionList { get; set; }
    }
}

