

namespace Infrastructure.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }

        public Guid FromAccountId { get; set; }
        public Account FromAccount { get; set; } = null!;

        public Guid ToAccountId { get; set; }
        public Account ToAccount { get; set; } = null!;

        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public TransactionStatus Status { get; set; } = TransactionStatus.Completed;


        public static WalletTransaction Create(Guid fromId, Guid toId, decimal amount, string desc)
        {
            return new WalletTransaction
            {
                Id = Guid.NewGuid(),
                FromAccountId = fromId,
                ToAccountId = toId,
                Amount = amount,
                Description = desc,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkCompleted() => Status = TransactionStatus.Completed;
    }
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed
    }

}
