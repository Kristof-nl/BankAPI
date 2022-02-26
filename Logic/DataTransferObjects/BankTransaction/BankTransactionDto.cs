using System;

namespace Logic.DataTransferObjects.BankTransactions
{
    public class BankTransactionDto
    {
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Ammount { get; set; }
        public double AmmountBefore { get; set; }
        public double AammountAfter { get; set; }
        public Data.DataObjects.Bank Bank { get; set; }
    }
}
