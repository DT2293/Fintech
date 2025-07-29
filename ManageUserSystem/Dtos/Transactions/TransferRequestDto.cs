﻿namespace ManageUserSystem.Dtos.Transactions
{
    public class TransferRequestDto
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
