using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface ICheckerRules
    {
        void CheckCountLeadTransactions(LeadForUpdateRole _lead, int countTransactionsWithoutWithdraw, CancellationTokenSource cancellationTokenSource);
        void CheckDifferenceWithdrawDeposit(LeadForUpdateRole _lead, List<TransactionResponseModel>? leadTransactionsLastMonthWithdrawDeposit, CancellationTokenSource cancellationTokenSource);
        void CheckLeadBirthday(LeadForUpdateRole _lead, CancellationTokenSource cancellationTokenSource);
    }
}