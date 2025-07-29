using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Repositories.AccountRepo;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class TransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task TransferAsync(Guid fromId, Guid toId, decimal amount, string description)
    //{
    //    await _unitOfWork.BeginTransactionAsync();

    //    try
    //    {
    //        var fromAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(fromId)
    //            ?? throw new InvalidOperationException("Không tìm thấy tài khoản nguồn");

    //        var toAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(toId)
    //            ?? throw new InvalidOperationException("Không tìm thấy tài khoản đích");

    //        if (fromAccount.CurrencyId != toAccount.CurrencyId)
    //            throw new InvalidOperationException("Khác loại tiền tệ");

    //        if (fromAccount.Balance < amount)
    //            throw new InvalidOperationException("Số dư không đủ");

    //        var transaction = new WalletTransaction
    //        {
    //            Id = Guid.NewGuid(),
    //            FromAccountId = fromId,
    //            ToAccountId = toId,
    //            Amount = amount,
    //            Description = description,
    //            Status = TransactionStatus.Pending,
    //            CreatedAt = DateTime.UtcNow
    //        };

    //        await _unitOfWork.Transactions.AddAsync(transaction);

    //        fromAccount.Balance -= amount;
    //        toAccount.Balance += amount;

    //        await _unitOfWork.Accounts.UpdateBalanceAsync(fromAccount);
    //        await _unitOfWork.Accounts.UpdateBalanceAsync(toAccount);

    //        // Không gọi UpdateAsync(transaction) vì EF đang track transaction
    //        transaction.Status = TransactionStatus.Completed;

    //        await _unitOfWork.SaveChangesAsync();
    //        await _unitOfWork.CommitAsync();
    //    }
    //    catch
    //    {
    //        await _unitOfWork.RollbackAsync();
    //        throw;
    //    }
    //}

    public async Task TransferAsync(Guid fromId, Guid toId, decimal amount, string description)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var fromAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(fromId)
                ?? throw new InvalidOperationException("Không tìm thấy ví nguồn");

            var toAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(toId)
                ?? throw new InvalidOperationException("Không tìm thấy ví đích");

            if (fromAccount.CurrencyId != toAccount.CurrencyId)
                throw new InvalidOperationException("Khác loại tiền tệ");

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            var transaction = WalletTransaction.Create(fromId, toId, amount, description);

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.Accounts.UpdateBalanceAsync(fromAccount);
            await _unitOfWork.Accounts.UpdateBalanceAsync(toAccount);

            transaction.MarkCompleted();

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

}

