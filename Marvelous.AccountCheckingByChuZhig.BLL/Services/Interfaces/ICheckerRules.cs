using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface ICheckerRules
    {
        bool CheckDifferenceWithdrawDeposit(List<TransactionResponseModel> leadTransactionsLastMonth);
        bool CheckLeadBirthday(LeadModel leadModel);
        bool CheckCountLeadTransactions(int countTransactionsWithoutWithdraw);
    }
}