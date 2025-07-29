namespace Infrastructure.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;

        public decimal Balance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Số tiền không hợp lệ");
            if (Balance < amount) throw new InvalidOperationException("Số dư không đủ");
            Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Số tiền không hợp lệ");
            Balance += amount;
        }
    }
}
