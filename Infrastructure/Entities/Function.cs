namespace Infrastructure.Entities
{
    public class Function
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; } = null!; 
        public string? UpdatedBy { get; set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
