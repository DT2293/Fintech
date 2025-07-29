namespace Infrastructure.Entities
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!; // VND, USD, ...
        public string Name { get; set; } = null!;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
