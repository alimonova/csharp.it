using System;
namespace csharp_it.Models
{
    public class Transaction
    {
        public string transactionType { get; set; }
        public string orderReference { get; set; }
        public string createdDate { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string baseAmount { get; set; }
        public string baseCurrency { get; set; }
        public string transactionStatus { get; set; }
        public string reason { get; set; }
        public string reasonCode { get; set; }
        public string settlementDate { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string paymentSystem { get; set; }
        public string cardPan { get; set; }
        public string cardType { get; set; }
        public string issuerBankCountry { get; set; }
        public string issuerBankName { get; set; }
        public string fee { get; set; }
    }
}

