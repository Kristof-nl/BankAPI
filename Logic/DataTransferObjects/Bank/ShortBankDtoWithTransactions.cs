using Logic.DataTransferObjects.BankTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.DataTransferObjects.Bank
{
    public class ShortBankDtoWithTransations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ShortBankTransactionDto> BankTransactions { get; set; }
    }
}
