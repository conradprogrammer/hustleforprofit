using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Models
{
    public class TransactionModel
    {
        public string UserEmail { get; set; }   
        public int RealPointAmount { get; set; }
        public int RealCreditAmount { get; set; }
        public GlobalEnums.TransactionTypeID TransactionTypeID { get; set; }    
        public string TransactionDescription { get; set; }  

    }
}
