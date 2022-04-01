﻿using Marvelous.AccountCheckingByChuZhig.BLL.Models;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Services
{
    public interface IReportService
    {
        Task<List<TransactionResponseModel>?> GetLeadTransactionsForPeriod(int leadId, DateTime startDate, DateTime endDate);
        Task<List<LeadModel>?> NewGetAllLeads(int start, int amount, CancellationTokenSource cancellationTokenSource);
        Task<int> GetCountRANDOmLeadTransactionsWithoutWithdraw(int leadId);
        Task<int> GetCountLeadTransactionsWithoutWithdrawal(int leadId, DateTime startDate, CancellationTokenSource cancellationTokenSource);
        Task<List<TransactionResponseModel>?> GetLeadTransactionsDepositWithdrawForLastMonth(int leadId, CancellationTokenSource cancellationTokenSource);
    }
}