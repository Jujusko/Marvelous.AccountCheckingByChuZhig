using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Worker
{
    public interface ICheckerRules
    {
        bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel> leadTransactionsLastMonth);
        bool CheckLeadBirthday(LeadModel leadModel);
        bool CheckLeadTransactions(List<TransactionResponseModel> leadTransactions);
    }
}